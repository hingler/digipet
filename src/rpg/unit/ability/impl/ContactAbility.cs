using System.Numerics;
using digipet.rpg.context;
using digipet.rpg.util;

namespace digipet.rpg.unit.ability.impl;

#nullable enable

public class ContactAbility : IAbility {
  public AbilitySpread Spread => AbilitySpread.SINGLE;
  public UnitTeam Target => UnitTeam.ENEMY;
  public AbilityFormat Format => AbilityFormat.Physical;

  public double DamageFactor = 1.0;
  public double KnockbackFactor = 1.0;

  public double NetCooldown { get; set; } = 0.001;

  private readonly MinDistanceTester tester;

  public ContactAbility() {
    tester = new() {
      MaxCastDistance = 0.01
    };
  }

  public virtual bool Cast(ICharContext context, ICharState? opp = null) {
    if (tester.CanCast(context, opp)) {
      ICharState opponent = opp!;
      context.Attack(DamageFactor, KnockbackFactor, opponent);
      return true;
    }

    return false;
  }
}