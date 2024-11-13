using System;
using digipet.util;

namespace digipet.sim.edible;


public class SimpleTasteModel : ITasteModel {
  private readonly Random random = new();
  public double GetTaste(IEdiblePickup pickup) {
    double mean = pickup.Appeal;
    double stdev = pickup.Variance;

    return Gaussian.GetGaussian(random) * stdev + mean;
    
  }
}