using digipet.rpg.context;

namespace digipet.rpg.model;

// calculates advancement speed
public interface ISpeedCalculator {
  // absolute cap
  double GetAccelRate(ICharStats stats);

  // absolute cap on movement speed
  double GetMaxSpeed(ICharStats stats);

  double GetKnockbackDelta(ICharState state, double raw_knockback);
}