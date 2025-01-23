using digipet.sim.edible;
using digipet.sim.water;

namespace digipet.sim;

public interface ISimComponent {
  // expect sim components to tick 1x per second
  // ticks should assume normal decay only
  public void Tick(int tick_seconds);
}

public interface IHungerModel : ISimComponent {
  double Fullness { get; set; }

  // returns true if the food can be eaten
  public bool CanEat(IEdiblePickup food);

  // eats, returns "canEat"
  public bool Eat(IEdiblePickup food, out double taste);
}

public interface IThirstModel : ISimComponent {
  double Quenchiness { get; set; }

  public double Drink(double units, IWaterSource source);
}