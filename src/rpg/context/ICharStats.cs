using digipet.rpg.unit;

namespace digipet.rpg.context;

public interface ICharStats {
  public long Attack { get; }
  public long Wisdom { get; }
  public long Defense { get; }
  public long Weight { get; }
  public long Vitality { get; }
  public long Speed { get; }

  // width of this char in units
  public float Width { get; }

  IReadOnlyList<IAbility> GetAbilities();
}