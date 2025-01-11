using digipet.rpg.context;

namespace digipet.rpg.unit;

#nullable enable

public interface IAbility {
  AbilitySpread Spread { get; }
  UnitTeam Target { get; }
  AbilityFormat Format { get; }

  // total cooldown time
  double NetCooldown { get; }

  // how do we want to do targets?
  // - let each ability calculate the best target on its own? (thinking so! if relevant)
  public bool Cast(ICharContext context, ICharState? target = null);
}

public interface ICharAbility : IAbility {
  // time before ability can be used next
  double Cooldown { get; }
}