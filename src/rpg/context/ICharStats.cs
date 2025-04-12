using digipet.rpg.unit;

namespace digipet.rpg.context;

public interface ICharStats : ICharStatsBase {

  // width of this char in units (should this be in stats??)
  public float Width { get; }

  IReadOnlyList<IAbility> GetAbilities();
}