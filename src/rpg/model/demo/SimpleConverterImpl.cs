using digipet.rpg.context;
using digipet.transition.animator;
using digipet.util;

namespace digipet.rpg.model.demo;

public class SimpleConverterImpl : IMaxHPCalculator, IPowerConverter, ISpeedCalculator {
  public long GetMaxHP(ICharStats stats) {
    return (long)(Math.Pow(1.01, stats.Vitality / 15) * 348 + (1 + stats.Vitality / 100) * 652);
  }

  public double ToRawDamage(ICharStats stats, double damage_fac) {
    // force imparted - arb level
    // avg around "1.0" ? weight differently if needed
    return (stats.Attack * 0.82 + stats.Vitality * 0.04 + stats.Wisdom * 0.14) * damage_fac;
  }

  public double ToNetDamage(ICharStats stats, double raw_damage) {
    // fine with weighting this gt one - should be resistance to raw force
    return raw_damage / (stats.Weight * 4.5 + stats.Defense * 18.4 + stats.Vitality * 2.7);
  }

  // thinking: add func for computing magic damage, instead of atk damage
  // (considerable weight on wisdom)

  public double ToRawKnockback(ICharStats stats, double knockback_fac) {
    // force imparted - think blunt force vs slicing strength
    return (stats.Attack * 0.5 + stats.Weight * 0.35 + stats.Vitality * 0.15) * knockback_fac;
  }


  public double GetAccelRate(ICharStats stats) {
    // idea: accel rate should be fixed to some theoretical "ideal" w/ diminishing returns
    // falling below a certain threshold begins to slow accel rate considerably
    const double CEIL_ACCEL = 45.0;
    const double MAX_ACCEL = 35.0;
    const double MIN_ACCEL = 16.0;
    // gt this > 1.0
    const double ACCEL_RATIO_CAP = 1.2;

    double char_accel_ratio = stats.Weight / Math.Max(stats.Speed, 0.001);
    
    if (char_accel_ratio < ACCEL_RATIO_CAP) {
      double excess = stats.Speed - (stats.Weight / ACCEL_RATIO_CAP);
      return MAX_ACCEL + Math.Min(excess * 0.004, CEIL_ACCEL - MAX_ACCEL);
    } else {
      double speed_weight_fac = Math.Log(char_accel_ratio) / Math.Log(Math.Max(ACCEL_RATIO_CAP, 1.0001));

      // arb slowdown factor
      return Math.Clamp(MAX_ACCEL - speed_weight_fac * 2.1, MIN_ACCEL, MAX_ACCEL);
    }
  }

  public double GetMaxSpeed(ICharStats stats) {
    const double MAX_SPEED_EXCESS = 16.0;
    const double MAX_SPEED = 9.0;
    const double MIN_SPEED = 5.0;

    const double FALLOFF_CAP = 1.6;

    // min speed threshold is (weight / 1.6)

    double char_speed_ratio = stats.Weight / Math.Max(stats.Speed, 0.0001);
    if (char_speed_ratio < FALLOFF_CAP) {
      // thinking: as speed gets higher add a *slight* buff
      double excess_speed = stats.Speed - (stats.Weight / FALLOFF_CAP);
      // reuse weight to get some approximation of excess speed?
      double speed_to_weight = stats.Speed / stats.Weight;
      // speed
      return MAX_SPEED + Math.Min(excess_speed * speed_to_weight * 0.003, MAX_SPEED_EXCESS - MAX_SPEED);
    } else {
      double speed_decrement = (char_speed_ratio - FALLOFF_CAP) * 2.5;
      return Math.Clamp(MAX_SPEED - speed_decrement, MIN_SPEED, MAX_SPEED);
    }
  }

  public double GetKnockbackDelta(ICharState state, double raw_knockback) {
    const double MIN_KNOCKBACK_FAC = 1.3;
    const double MAX_KNOCKBACK_FAC = 2.8;
    const double MIN_KNOCKBACK_ABS = 3.3;
    const double MAX_KNOCKBACK_ABS = 24.0;
    
    double intent_direction = (state.Team == UnitTeam.ALLY) ? 1.0 : -1.0;
    double position_advantage = state.Position.X * intent_direction;

    double cur_speed = Math.Max(Math.Abs(state.Velocity.X), 5.0);

    // increase knockback taken when we have advantage
    // decrease knockback taken when we have disadvantage
    double knockback_factor = 0.75 + 0.5 * EasingFunctions.SmoothStep(-6, 6, position_advantage);

    // infer from velocity, since we move in opp direction

    // if velocity is moving with 

    // i'm contented with these numbers
    double speed_delta_fac = 1.3 * (raw_knockback / (state.Stats.Weight * 0.9 + state.Stats.Defense * 0.3));

    double knockback_delta = Math.Clamp(speed_delta_fac, MIN_KNOCKBACK_FAC, MAX_KNOCKBACK_FAC) * cur_speed + MIN_KNOCKBACK_ABS;
    double knockback_final = Math.Clamp(knockback_delta, cur_speed + MIN_KNOCKBACK_ABS, cur_speed + MAX_KNOCKBACK_ABS);

    // speed we're shot in opposite direction
    double knockback_res = knockback_final - cur_speed;

    // multiply by our comp factor
    knockback_res *= knockback_factor;
    return knockback_res + cur_speed;
  }
}