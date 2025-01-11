using digipet.rpg.context;

namespace digipet.rpg.model;

public interface IMaxHPCalculator {
  long GetMaxHP(ICharStats stats);
}