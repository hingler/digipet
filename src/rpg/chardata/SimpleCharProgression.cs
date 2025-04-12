using digipet.rpg.unit;
using digipet.rpg.unit.ability.impl;

namespace digipet.rpg.chardata;

public class SimpleCharProgression : ICharProgression {
  public long Experience { get; set; } = 0;
  public int Level { get; } = 1;

  public IReadOnlyList<IAbility> GetAbilities() {
    return [ new ContactAbility() ]; 
  }
}