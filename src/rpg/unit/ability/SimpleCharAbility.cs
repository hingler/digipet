using digipet.rpg.context;

namespace digipet.rpg.unit.ability;

#nullable enable

public class SimpleCharAbility : ICharAbility {
  private readonly IAbility base_ability;

  private double cooldown;

  public AbilitySpread Spread => base_ability.Spread;
  public UnitTeam Target => base_ability.Target;
  public AbilityFormat Format => base_ability.Format;
  public double NetCooldown => base_ability.NetCooldown;
  public double Cooldown { get => Math.Max(cooldown, 0.0); set => cooldown = value; }

  public SimpleCharAbility(IAbility base_ability) {
    this.base_ability = base_ability;
    cooldown = NetCooldown;
  }

  public bool Cast(ICharContext context, ICharState? opp = null) {
    if (cooldown > 0.001) {
      return false;
    }

    if (base_ability.Cast(context, opp)) {
      cooldown = NetCooldown;
      return true;
    }

    return false;
  }

  // who tf is gonna call this
  public void Tick(double delta) {
    cooldown -= delta;
  }
}