using digipet.rpg.context;

namespace digipet.rpg.unit;

// should buffs be consistently multiplicative, or sometimes-additive?
public interface ICharBuff {
  // stats field defining multiplicative factors
  ICharStats BuffMult { get; }
  ICharStats BuffAdd { get; }

  // time remaining before buf expires
  // (tba: need to instantiate - thinking of logging this as a spriteless "combat entity" so we receive ticks)
  double TimeRemaining { get; }
}