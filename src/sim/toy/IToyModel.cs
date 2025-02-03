using digipet.sim.toy.impl;

namespace digipet.sim.toy;

// idea:
// - simply fetch the "interest value" presently associated with a given toy
// - 0 -> no impact
// - gt 0 -> + fun
// - lt 0 -> - fun
public interface IToyModel : ISimComponent {
  // time snapshot
  ToyModelData AsData();
  double GetRawInterest(IToy toy);
  bool IsActive(IToy toy);

  // give interest only, and let toy model figure out what to consume from
  double ConsumeInterest(IToy toy, double amt);
  IEnumerable<IToyActivator> GetSpawnedToys();
  IEnumerable<IToyActivator> GetActiveToys();
}