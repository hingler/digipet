using digipet.rpg.context;

namespace digipet.rpg.model;

public interface IBehaviorModel {
  void Tick(double delta, ICharContext context);

  void OnCollide(ICharState opp, ICharContext context);
}