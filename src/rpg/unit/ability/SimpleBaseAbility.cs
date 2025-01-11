using digipet.rpg.context;

namespace digipet.rpg.unit.ability;

#nullable enable

public class SimpleBaseAbility : IAbility {
  public AbilitySpread Spread { get; set; } = AbilitySpread.SINGLE;
  public UnitTeam Target { get; set; } = UnitTeam.ENEMY;
  public AbilityFormat Format { get; set; } = AbilityFormat.Physical;

  public double NetCooldown { get; set; } = 0.001;
  public virtual bool Cast(ICharContext context, ICharState? opp = null) {
    if (opp != null) {
      context.Attack(1.0, 1.0, opp);
    }

    return opp != null;
  }
}