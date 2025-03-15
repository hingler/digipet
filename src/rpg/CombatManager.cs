using System.Numerics;
using digipet.rpg.context;
using digipet.rpg.context.simple;
using digipet.rpg.entity;
using digipet.rpg.model.demo;
using digipet.rpg.unit;
using digipet.rpg.util;
using digipet.util;
using digipet.world;

namespace digipet.rpg;

#nullable enable

public class CombatManager : ICombatHook, IWorldManager {
  // BM's send events thru context and up here
  // crunch from "ability" -> "raw dmg / raw knockback"
  // crunch from "raw dmg / raw knockback" to "net dmg / delta-v"

  // behavior model should be bypassed altogether on both of these
  // but, on collide, we want to let the behavior model decide what to do


  // how it works

  // fire collide events to behavior model (onCollide)
  // fire update events to behavior model (Tick(delta))

  // behavior model interacts with hook, and targets enemies
  // combat manager relays that behavior to the char states
  // char states run their processors to manage the hit

  private readonly List<SimpleCharState> allies;
  private readonly List<SimpleCharState> enemies;

  private readonly HashSet<IWorldEntity> combat_entities;

  private readonly SimpleConverterImpl converter;

  private static readonly ILogger logger = LoggerSingleton.GetStaticLogger<CombatManager>();

  private readonly Random r = new();

  private int TickFactor = 0;

  // thinking: 1, 2, 3...
  // 1 is real time (half speed)
  // 2 is double speed
  // 3 is 4x
  public int Speed {
    get => TickFactor + 2;
    set => TickFactor = Math.Clamp(value, 0, 4) - 2;
  }

  public CombatManager(
    ICollection<CharInfo> stats_allies,
    ICollection<CharInfo> stats_enemies
  ) {
    allies = [];
    enemies = [];
    combat_entities = [];

    converter = new();

    int counter = 0;
    foreach (CharInfo stats in stats_allies) {
      SimpleCharState cs = new(
        stats.Stats, UnitTeam.ALLY, converter, converter, converter, new(-10.0f + (counter++), 0.0f)
      ) {
        BehaviorModel = stats.BehaviorModel
      };

      CreateEntity(new CharStateEntity(cs, stats.CharSprite));

      allies.Add(cs);

    }

    counter = 0;

    foreach (CharInfo stats in stats_enemies) {
      SimpleCharState cs = new(
        stats.Stats, UnitTeam.ENEMY, converter, converter, converter, new(10.0f - (counter++), 0.0f)
      ) {
        BehaviorModel = stats.BehaviorModel
      };

      CreateEntity(new CharStateEntity(cs, stats.CharSprite));

      enemies.Add(cs);
    }
  }

  public IEnumerable<IWorldEntity> GetEntities() {
    return combat_entities;
  }

  public void Tick(double delta_in) {

    int tick_count;
    double delta;
    if (TickFactor < 0) {
      tick_count = 1;
      // slow down by 2 ^ (-tick_factor)
      delta = delta_in / (1 << -TickFactor);
    } else {
      // if 0: 1
      // if > 0: 2 ^ (tick_factor)
      tick_count = 1 << TickFactor;
      delta = delta_in;
    }

    for (int i = 0; i < tick_count; i++) {
      
      foreach (SimpleCharState cs in allies) {
        UpdateCharStates(cs, delta);
      }

      foreach (SimpleCharState cs in enemies) {
        UpdateCharStates(cs, delta);
      }

      UpdateEntities(delta);

      HandleCharCollisions();
    }
  }

  private void UpdateEntities(double delta) {

    foreach (IWorldEntity entity in combat_entities) {
      entity.Tick(delta);
    }
    
    combat_entities.RemoveWhere(m => !m.Active);
  }

  public void UpdateCharStates(SimpleCharState cs, double delta) {
    // runs tick, polls direction from ctx, runs some math and updates pos
    cs.Tick(delta);
    
    SimpleCharContext cc = new(this, cs);
    cs.BehaviorModel.Tick(delta, cc);

    // cap bkd speed
    double world_direction = Math.Max(cc.Direction, -0.2) * (cs.Team == UnitTeam.ALLY ? 1 : -1);
    double accel_rate = converter.GetAccelRate(cs.Stats);
    double speed_cap = converter.GetMaxSpeed(cs.Stats);

    double speed_target = converter.GetMaxSpeed(cs.Stats) * world_direction;
    double accel_delta = speed_target - cs.Velocity.X;

    double accel_net = Math.Clamp(accel_delta, -1, 1) * accel_rate;

    //
    double velocity_tick = cs.Velocity.X + (float)(accel_net * delta);
    // keep it positive until the very end
    double new_velocity = Math.Clamp(velocity_tick, -speed_cap, speed_cap);

    if (Math.Abs(velocity_tick) > speed_cap) {
      double dt = Math.Exp(delta * -3.0);
      new_velocity = new_velocity * (1.0 - dt) + velocity_tick * dt;
    }

    cs.Velocity = new((float)new_velocity, 0.0f);

    cs.Position += cs.Velocity * (float)delta;
  }

  public void HandleCharCollisions() {
    Dictionary<SimpleCharState, Vector2> enemy_bounds = [];
    foreach (SimpleCharState cs_enemy in enemies) {
      enemy_bounds.Add(cs_enemy, RPGUtil.GetBounds(cs_enemy));
    }

    foreach (SimpleCharState cs in allies) {
      // check if an ally is colliding with some enemy
      // if so: set velocity to 0, THEN apply knockback (ie so they bounce)
      Vector2 b = RPGUtil.GetBounds(cs);
      foreach (KeyValuePair<SimpleCharState, Vector2> kv in enemy_bounds) {
        // if the two intersect: call "onhit" on each w/ the other as its target
        Vector2 be = kv.Value;
        Vector2 bi = RPGUtil.TestBounds(b, be);
        if (bi.Y >= bi.X) {
          SimpleCharState cs_enemy = kv.Key;
          logger.Log("collision btwn ", b, " and ", be);
          HandleCollision(cs, cs_enemy, bi);
        }

      }
    }
  }

  private void HandleCollision(SimpleCharState csa, SimpleCharState csb, Vector2 intersect_vector) {
    CollisionCombatHook hook = new(converter, converter, this);

    SimpleCharContext context_a = new(hook, csa);
    SimpleCharContext context_b = new(hook, csb);

    // hook 

    // swap out the handles here with our collision hook
    // call "oncollide" on each
    // store the hits, tweak velocities ourselves
    // then: broadcast "onhit"

    // slide so that they're no longer colliding

    float intersect_dist = (intersect_vector.Y - intersect_vector.X) / 2;

    // assumption: csa is on the left, csb is on the right (which holds true for now!)
    csa.Position = new(csa.Position.X - intersect_dist, csa.Position.Y);
    csb.Position = new(csb.Position.X + intersect_dist, csb.Position.Y);

    // handle oncollide under the assumption that they're smack dab touching

    // (should separate these, this is hacky)
    csa.BehaviorModel.OnCollide(csb, context_a);
    csb.BehaviorModel.OnCollide(csa, context_b);

    AttackData data_a = new();
    AttackData data_b = new();

    foreach (var a in hook.GetAttacks()) {
      IDetailedCharState actor = a.Item1;
      AttackData spec = a.Item3;

      List<SimpleCharState> targets = GetTargets(actor, a.Item2);
      foreach (SimpleCharState target in targets) {
        if (target == csa) {
          data_a.RawDamage += spec.RawDamage;
          data_a.RawKnockback += spec.RawKnockback;
        } else if (target == csb) {  // target == csb
          data_b.RawDamage += spec.RawDamage;
          data_b.RawKnockback += spec.RawKnockback;
        } else {
          HandleSingleAttack(target, spec.RawDamage, spec.RawKnockback);
        }
      }
    }

    // whats next?

    // - start filling out gameplay features - this logic seems OK
    // do some refactoring?
    // doodle out some class specs?
    // - ie: make up some classes to brawl for us
    // damage numbers
    // health bars
    // win/loss conditions

    double momentum_a = GetMomentumKnockback(csa) * 0.14;
    double momentum_b = GetMomentumKnockback(csb) * 0.14;


    // momentum imparted - momentum initial
    data_a.RawKnockback = Math.Max(data_a.RawKnockback + momentum_b - momentum_a, 0.1);
    data_b.RawKnockback += Math.Max(data_b.RawKnockback + momentum_a - momentum_b, 0.1);

    // impl some logic st knockback is lessened as we move from one side to the other


    csa.Velocity = Vector2.Zero;
    csb.Velocity = Vector2.Zero;

    logger.Log("net knockback: ", data_a.RawKnockback, ", ", data_b.RawKnockback);

    HandleSingleAttack(csa, data_a.RawDamage, data_a.RawKnockback);
    HandleSingleAttack(csb, data_b.RawDamage, data_b.RawKnockback);
  }

  private double GetMomentumKnockback(SimpleCharState c) {
    return c.Stats.Weight * c.Velocity.X * (c.Team == UnitTeam.ALLY ? 1 : -1);
  }

  public void CreateEntity(IWorldEntity entity) {
    combat_entities.Add(entity);
  }

  private List<SimpleCharState> GetTargets(IDetailedCharState actor, ICharState? target) {
    List<SimpleCharState> targets;
    if (target == null) {
      targets = GetEnemies_internal(actor);
    } else if (target is SimpleCharState css) {
      targets = [ css ];
    } else {
      targets = [];
    }

    return targets;
  }

  // combat hook impl

  public void EnqueueAttack(IDetailedCharState actor, double damage_fac, double knockback_fac, ICharState? target = null) {
    List<SimpleCharState> targets = GetTargets(actor, target);

    double raw_damage = converter.ToRawDamage(actor.Stats, damage_fac);
    double raw_knockback = converter.ToRawKnockback(actor.Stats, knockback_fac);

    logger.Log("attack on: ", targets);

    foreach (SimpleCharState state in targets) {
      // separate out knockback
      HandleSingleAttack(state, raw_damage, raw_knockback);
    }
  }

  private void HandleSingleAttack(SimpleCharState target, double raw_damage, double raw_knockback) {
    target.OnHit(raw_damage);
    target.Velocity += converter.KnockbackToDeltaV(target, raw_knockback);
  }

  

  public void EnqueueBuff(IDetailedCharState actor, ICharBuff buff, ICharState? target = null) {
    List<SimpleCharState> buff_targets = GetTargets(actor, target);

    foreach (SimpleCharState state in buff_targets) {
      state.OnBuff(buff);
    }
  }

  public void EnqueueTeamBuff(IDetailedCharState actor, ICharBuff buff, UnitTeam team) {
    List<SimpleCharState> targets = (team == UnitTeam.ALLY) ? GetTeammates_internal(actor) : GetEnemies_internal(actor);

    foreach (SimpleCharState state in targets) {
      state.OnBuff(buff);
    }
  }

  public IEnumerable<IDetailedCharState> GetAllCharacters() {
    return [..allies, ..enemies];
  }

  private List<SimpleCharState> GetEnemies_internal(ICharState self) {
    return (self.Team == UnitTeam.ALLY) ? enemies : allies;
  }

  public IReadOnlyList<ICharState> GetEnemies(ICharState self) => GetEnemies_internal(self);

  private List<SimpleCharState> GetTeammates_internal(ICharState self) {
    return (self.Team == UnitTeam.ALLY) ? allies : enemies;
  }

  public IReadOnlyList<ICharState> GetTeammates(ICharState self) => GetTeammates_internal(self);
}