using digipet.rpg.context;
using digipet.rpg.unit;
using digipet.util;

namespace digipet.rpg.model.behavior;

public class GreedyChargeModel : IBehaviorModel {
  private readonly ILogger logger;
  public GreedyChargeModel() {
    logger = this.GetLogger();
  }

  public void Tick(double delta, ICharContext context) {
    context.Direction = 1.0f;

    IReadOnlyList<ICharAbility> abilities = context.GetAbilities();
    for (int i = 0; i < abilities.Count; i++) {
      if (abilities[i].Cast(context, null)) {
        logger.Log("casted ability ", i, ": ");
      }
    }
  }

  public void OnCollide(ICharState opp, ICharContext context) {
    bool ability_activated = false;
    IReadOnlyList<ICharAbility> abilities = context.GetAbilities();
    int ab = 0;
    while (!ability_activated && ab < abilities.Count) {
      // cast until done
      ability_activated |= abilities[ab++].Cast(context, opp);
    }
  }
}