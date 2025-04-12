using digipet.image;
using digipet.rpg.context;
using digipet.sim;

namespace digipet.rpg;

// implement ID here?
public interface ICharData : IWorldItem {
  // base stats - not buffs
  // also: stats at level 0!!!
  public ICharStatsBase BaseStats { get; }
  public string Name { get; }

  public CharClass Class { get; }
  public CombatStyle Style { get; }

  // fetch sprite? fetch behavior model?
  public CharInfo ToCharInfo();
}