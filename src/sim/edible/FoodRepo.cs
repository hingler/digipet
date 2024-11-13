using System.Collections.Generic;
using digipet.framework;
using digipet.sim.db;

namespace digipet.sim.edible;

#nullable enable

// tba: something to do with implementing sprite fetching
public class FoodRepo : ISimRepo<IEdiblePickup> {
  private readonly CSVRepo<IEdiblePickup> repo;

  // mapping sprite paths??

  public FoodRepo(IEngine engine) {
    repo = new(
      new FoodCSVConverter(),
      "asset/food_table.csv", 
      engine.GetResourceLoader()
    );
  }

  public IReadOnlyCollection<IEdiblePickup> GetEntries() {
    return repo.GetEntries();
  }

  public IReadOnlyCollection<int> GetDescriptors() {
    return repo.GetDescriptors();
  }

  public IEdiblePickup Fetch(int descriptor) {
    return repo.Fetch(descriptor);
  }

  public bool TryFetch(int descriptor, out IEdiblePickup? output) {
    return repo.TryFetch(descriptor, out output);
  }
}