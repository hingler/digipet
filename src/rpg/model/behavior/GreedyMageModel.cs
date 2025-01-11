using digipet.rpg.context;
using digipet.rpg.model;
using digipet.rpg.unit;
using digipet.rpg.util;
using digipet.util;

namespace digipet.rpg.model.behavior;

#nullable enable

// same deal
public class GreedyMageModel : IBehaviorModel {
  private readonly GreedyChargeModel collide_model = new();

  public void Tick(double delta, ICharContext context) {
    collide_model.Tick(delta, context);

    // maintain some dist (lets say 5 units for now)
    double min_dist = double.MaxValue;
    ICharState? min_char = null;
    foreach (ICharState state in context.GetEnemies()) {
      double enemy_dist = RPGUtil.GetDistance(context.Self, state);
      if (enemy_dist < min_dist) {
        min_dist = enemy_dist;
        min_char = state;
      }
    }

    double min_cast = GetMinCastDistance();
    double max_cast = GetMaxCastDistance();

    double dist_fract = Math.Clamp((min_dist - min_cast) / (max_cast - min_cast), 0.0, 1.0);

    // thinking: in the (0.25 - 0.75) range try to stand still
    // ie: stretch 0.75 down to 0.5
    // stretch 0.25 up to 0.5
    if (dist_fract < 0.25) {
      dist_fract *= 2;
    } else if (dist_fract > 0.75) {
      dist_fract = 1.0 - ((1.0 - dist_fract) * 2);
    }

    double direction_lerp = Lerper.Lerp(dist_fract, -1.0, 1.0);

    context.Direction = (float)direction_lerp;

    foreach (ICharAbility ability in context.GetAbilities()) {
      if (ability.Cooldown < 0.001 && min_char != null) {
        ability.Cast(context, min_char);
      }
    }
  }


  public void OnCollide(ICharState opp, ICharContext context) {
    // this guy does nothing! thats why we're having this issue!
    bool ability_activated = false;
    IReadOnlyList<ICharAbility> abilities = context.GetAbilities();
    int ab = 0;
    while (!ability_activated && ab < abilities.Count) {
      // cast until done
      ability_activated |= abilities[ab++].Cast(context, opp);
    }
  }

  private double GetMaxCastDistance() {
    return 5.5;
  }

  private double GetMinCastDistance() {
    return 2.5;
  }
}