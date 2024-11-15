using System;
using digipet.db;
using digipet.util;

namespace digipet.sim.edible;


public class SimpleTasteModel : ITasteModel {
  private readonly Random random = new();
  private readonly IDataStore tasteStore;

  public SimpleTasteModel(IDataStore store) {
    tasteStore = store;
  }
  public double GetTaste(IEdiblePickup pickup) {
    if (tasteStore.TryFetch(pickup.Name, out TasteData data)) {
      this.GetLogger().Log("taste fetched!");
      return data.Taste;
    }

    double mean = pickup.Appeal;
    double stdev = pickup.Variance;

    double taste = Math.Clamp(Gaussian.GetGaussian(random) * stdev + mean, -5.4, 5.4);

    tasteStore.Store(pickup.Name, new TasteData(taste));

    return taste;
  }
}