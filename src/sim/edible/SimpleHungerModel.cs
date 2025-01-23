using System;

namespace digipet.sim.edible;

// imagining: down the line some individuals get hungry slower
public class SimpleHungerModel : IHungerModel {
  // arb
  private static readonly int DECAY_TIME = 60 * 60 * 25;
  
  private double fullness_;
  private readonly ITasteModel tasteModel;
  public double Fullness {
    get => fullness_;
    set => fullness_ = value;
  }

  public SimpleHungerModel(
    double init,
    ITasteModel tasteModel
  ) {
    fullness_ = init;
    this.tasteModel = tasteModel;
  }

  public void Tick(int tick_seconds) {
    double net_decay = (double)tick_seconds / DECAY_TIME;

    fullness_ = Math.Clamp(Fullness - net_decay, 0.0, 1.0);
  }

  public bool CanEat(IEdiblePickup food) {
    double hunger_space = 1.0 - Fullness;
    // if we have space for food, then eat the whole thing
    return hunger_space > 0.5 * food.GetFill();
  }

  public bool Eat(IEdiblePickup food, out double taste) {
    bool eaten = CanEat(food);
    if (eaten) {
      fullness_ = Math.Clamp(Fullness + food.GetFill(), 0.0, 1.0);
    }

    taste = tasteModel.GetTaste(food);
    return eaten;
  }
}