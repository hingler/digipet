using digipet.rpg.model;
using digipet.rpg.unit;

namespace digipet.rpg.context.simple;

#nullable enable

using Attack = Tuple<IDetailedCharState, ICharState?, AttackData>;

// accumulate attacks
// poll net hits, net buffs
public class CollisionCombatHook : ICombatHook {
  private readonly List<Attack> attacks = [];

  private readonly IPowerConverter power_converter;
  private readonly ISpeedCalculator speed_calculator;
  private readonly ICombatHook hook_delegate;
  
  public CollisionCombatHook(
    IPowerConverter power_converter,
    ISpeedCalculator speed_calc,
    ICombatHook hook_delegate
  ) {
    this.power_converter = power_converter;
    this.speed_calculator = speed_calc;
    this.hook_delegate = hook_delegate;
  }

  public void CreateEntity(IWorldEntity entity) {
    hook_delegate.CreateEntity(entity);
  }

  public IEnumerable<Attack> GetAttacks() => attacks;

  public void EnqueueAttack(IDetailedCharState actor, double damage_fac, double knockback_fac, ICharState? target = null) {
    double raw_damage = power_converter.ToRawDamage(actor.Stats, damage_fac);
    double raw_knockback = power_converter.ToRawKnockback(actor.Stats, knockback_fac);
    // oh fuck how do we want to do this
    // the idea was just to give the context this "hook" and see how much damage is outputted

    AttackData data = new() {
      RawDamage = raw_damage,
      RawKnockback = raw_knockback
    };

    attacks.Add(new(actor, target, data));
  }



  public void EnqueueBuff(IDetailedCharState actor, ICharBuff buff, ICharState? target = null) {
    hook_delegate.EnqueueBuff(actor, buff, target);
  }

  public void EnqueueTeamBuff(IDetailedCharState actor, ICharBuff buff, UnitTeam team) {
    hook_delegate.EnqueueTeamBuff(actor, buff, team);
  }

  public IReadOnlyList<ICharState> GetEnemies(ICharState self) {
    return hook_delegate.GetEnemies(self);
  }

  public IReadOnlyList<ICharState> GetTeammates(ICharState self) {
    return hook_delegate.GetTeammates(self);
  }
}