using digipet.framework;
using digipet.rpg.context;
using digipet.rpg.entity;

namespace digipet.rpg.unit.ability.impl;

#nullable enable

public class MagicAbility : IAbility {
  public AbilitySpread Spread => AbilitySpread.SINGLE;
  public UnitTeam Target => UnitTeam.ENEMY;
  public AbilityFormat Format => AbilityFormat.Projectile;

  public double DamageFactor = 0.5;
  public double KnockbackFactor = 0.1;

  public double NetCooldown { get; set; } = 0.4;
  
  public double CastRange {
    get => tester.MaxCastDistance;
    set => tester.MaxCastDistance = value;
  }

  private readonly MinDistanceTester tester;
  private readonly IEngine engine;

  public MagicAbility(IEngine engine) {
    tester = new() {
      MaxCastDistance = 15.0
    };

    this.engine = engine;
  }

  public virtual bool Cast(ICharContext context, ICharState? opp = null) {
    if (tester.CanCast(context, opp)) {
      ICharState opponent = opp!;
      context.CreateEntity(
        new MagicBall(engine, context, opponent)
      );

      return true;
    }

    return false;
  }
}