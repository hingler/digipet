using digipet.rpg.context;
using digipet.rpg.util;

namespace digipet.rpg.unit.ability.impl;

#nullable enable

public class MinDistanceTester {
  public double MaxCastDistance = 0.001;

  public bool CanCast(ICharContext context, ICharState? opp = null) {
    if (opp == null) {
      return false;
    }

    ICharState opponent = opp!;

    // some arb contact distance
    return RPGUtil.GetDistance(context.Self, opponent) < MaxCastDistance;
  }
}