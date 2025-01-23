using digipet.component;
using digipet.framework;
using digipet.rpg;
using digipet.rpg.context.simple;
using digipet.rpg.model.behavior;
using digipet.rpg.model.demo;
using digipet.rpg.unit;
using digipet.rpg.unit.ability;
using digipet.rpg.unit.ability.impl;
using digipet.view.gpr;
using digipet.view.rpg;
using digipet.view.rpg.damage;

namespace digipet.scenes.demo;

public class CombatDemo : Scene {
  private readonly CombatManager manager;
  private readonly RPGEntityWrap viewer;
  private readonly RPGGrid grid;
  private readonly RPGDamageView damage_view;
  private readonly RPGEntityScaler scaler;

  public CombatDemo(IEngine engine) : base(engine) {
    // initialize manager with some default data
    // run our jit and see what happens

    // (alt2: run some test code lol)

    List<CharInfo> info_ally = [];
    List<CharInfo> info_enemy = [];

    for (int i = 0; i < 1; i++) {
      SimpleCharStats stats_temp = GetPlaceholderStat();
      stats_temp.Weight = 1500;
      stats_temp.Speed = 2400;
      stats_temp.Attack = 1800;
      CharInfo info = new() {
        Stats = stats_temp,
        CharSprite = engine.GetSpriteFetcher().GetSprite(sprite.attrib.SpriteID.STAT_FOOD),
        BehaviorModel = new GreedyMageModel()
      };

      stats_temp.Abilities.Add(new MagicAbility(engine) {});

      SimpleCharStats stats_tank = GetPlaceholderStat();
      stats_tank.Weight = 3500;
      stats_tank.Speed = 1400;
      stats_tank.Attack = 2250;

      CharInfo info_tank = new() {
        Stats = stats_tank,
        CharSprite = engine.GetSpriteFetcher().GetSprite(sprite.attrib.SpriteID.STAT_FOOD),
        BehaviorModel = new GreedyChargeModel()
      };

      info_ally.Add(info);
      info_ally.Add(info_tank);

      SimpleCharStats stats_enemy = GetPlaceholderStat();
      stats_enemy.Speed = 2500;
      stats_enemy.Attack = 3550;
      stats_enemy.Weight = 1800;
      stats_enemy.Defense = 1300;
      stats_enemy.Weight = 2100;

      info_enemy.Add(new() {
        Stats = stats_enemy,
        CharSprite = engine.GetSpriteFetcher().GetSprite(sprite.attrib.SpriteID.STAT_FOOD),
        BehaviorModel = new GreedyChargeModel()
      });
    }

    manager = new(info_ally, info_enemy) {
      Speed = 2
    };
    viewer = new(manager);
    grid = new();
    damage_view = new(manager);

    RPGDisplayDelegate d = new();
    d.SizePx = new(192.0f);

    d.AddDisplay(viewer);
    d.AddDisplay(grid);
    d.AddDisplay(damage_view);

    scaler = new(manager, d);
  } 

  public override void InitScene() {
    PushToStack(viewer);
    PushToStack(grid);
    PushToStack(damage_view);
  }

  public override void Tick(double delta) {
    base.Tick(delta);
    manager.Tick(delta);

    scaler.UpdateScale(delta);
  }

  private static SimpleCharStats GetPlaceholderStat() {
    SimpleCharStats stats = new() {
      Attack = 1000,
      Defense = 1000,
      Speed = 1000,
      Wisdom = 1000,
      Vitality = 1000,
      Weight = 1000,
      Width = 0.6f,
    };

    IAbility sample_ability = new ContactAbility() {
      NetCooldown = 0.001
    };


    stats.Abilities.Add(sample_ability);

    return stats;
  }
}