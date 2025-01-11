using digipet.rpg.context;
using digipet.rpg.unit;
using digipet.util;

namespace digipet.rpg.model.demo;

public class TrivialBehaviorModel : IBehaviorModel {
  public TrivialBehaviorModel() {}

  public void Tick(double delta, ICharContext context) {
    context.Direction = 1.0f;
  }

  public void OnCollide(ICharState opp, ICharContext context) {
    // cast 0 ability on collision
    IReadOnlyList<ICharAbility> abilities = context.GetAbilities();
    bool res = abilities[0].Cast(context, opp);
    LoggerSingleton.GetLogger().Log("cast result on ", opp, ": ", res);
  }
}