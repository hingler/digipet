using digipet.rpg.unit;

namespace digipet.rpg.chardata;

public interface ICharProgression {
  public long Experience { get; }
  public int Level { get; }

  // arbitrary, char-to-char
  public IReadOnlyList<IAbility> GetAbilities();
}