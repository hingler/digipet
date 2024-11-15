namespace digipet.sim.water;

public interface IWaterData {
    // amount of water the source can hold, in arb units
  double Capacity { get; }

  // amount of water in same units
  double Contents { get; }
}

public interface IWaterSource : IWaterData {
  // fills this water source. returns true if space available - false if flooding
  bool Fill(double quantity);

  // attempts to drink *units* of water
  // @returns number of units actually consumed
  double Drink(double units);

  IWaterData AsWaterData() {
    return new WaterData(this);
  }
}