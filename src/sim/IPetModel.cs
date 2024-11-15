using digipet.file.stream;
using digipet.sim.edible;
using digipet.sim.impl;
using digipet.sim.water;

namespace digipet.sim;

public interface IPetData {
  double Food { get; }
  double Water { get; }
  double Fun { get; }
  double Social { get; }
  double Energy { get; }
  long PetExp { get; }
  string PetName { get; }
}

// this works for now
// thinking: we'll come up with some better way to pass in items, rather than modifying stats ourselves
public interface IPetModel : IPetData {

  bool CanEat(IEdiblePickup food_item);
  // eats the food item and returns a desirability score
  // 0.0 is avg - + is good, - is bad
  bool TryEat(IEdiblePickup food_item, out double score);
  double Drink(double units, IWaterSource source);

  IPetData AsPetData() {
    return new PetData(this);
  }
}