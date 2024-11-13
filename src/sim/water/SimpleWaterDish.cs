using System;

namespace digipet.sim.water;

// 
public class SimpleWaterDish : IWaterSource {
  public double Capacity { get; set; }
  public double Contents { get; set; }
  public SimpleWaterDish(double initCapacity, double initContents) {
    Capacity = initCapacity;
    Contents = initContents;
  }

  public bool Fill(double quantity) {
    // how to pass the source down?
    bool overflow = (Contents + quantity) > Capacity;
    Contents = Math.Clamp(quantity + Contents, 0.0, Capacity);
    return overflow;
  }

  public double Drink(double units) {
    double consume = Math.Min(units, Contents);
    Contents = Math.Max(Contents - consume, 0.0);

    return consume;
  }
}