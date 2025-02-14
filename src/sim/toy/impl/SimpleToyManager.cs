using digipet.framework;
using digipet.sim.db;
using static digipet.util.Closure;

namespace digipet.sim.toy;

#nullable enable

// alt:
// - manager puts toys in sim world
// - model fetches toys from sim world
// activators need to expose associated toy data

public class SimpleToyManager : IToyManager {
  private readonly ISimRepo<IToyFactory>? factory_repo;

  private readonly Dictionary<IToyFactory, BaseToyHandler> spawned_toys = [];

  private readonly IEngine engine;

  public SimpleToyManager(IEngine engine) {
    this.engine = engine;
    factory_repo = engine.GetAssetRepo<IToyFactory>();
  }

  public void Tick(double delta) {
    foreach (BaseToyHandler handler in spawned_toys.Values) {
      handler.Tick(delta);
    }
  }

  public bool IsSpawned(int rid) {
    return spawned_toys.Where(b => b.Key.RID == rid).Any();
  }

  public IEnumerable<IToyFactory> GetAvailableToys() 
    => factory_repo?.GetEntries() ?? [];

  public IEnumerable<IToy> GetSpawnedToys() {
    return spawned_toys.Keys;
  }

  public void ToggleToy(int toy_rid) {
    if (factory_repo == null) {
      return;
    }

    foreach (KeyValuePair<IToyFactory, BaseToyHandler> toy in spawned_toys) {
      if (toy.Key.RID == toy_rid) {
        DestroyToy(toy_rid);
        return;
      }
    }

    CreateToy(toy_rid);
  }

  // creates toy, if not already exists
  public BaseToyHandler? CreateToy(int toy_rid) {
    if (factory_repo == null) {
      return null;
    }

    if (factory_repo.TryFetch(toy_rid, out IToyFactory? factory)) {
      if (spawned_toys.TryGetValue(factory!, out BaseToyHandler? handler)) {
        return handler;
      }

      if (factory != null) {
        BaseToyHandler handler_new = factory.Create(engine);
        spawned_toys[factory] = handler_new;
        return handler_new;
      }
    }

    // putting things away:
    // - make the user clean up.

    return null;
  }

  // something like, grey out the toys that are already active

  public bool DestroyToy(int toy_rid) {
    if (factory_repo == null) {
      return false;
    }

    if (factory_repo.TryFetch(toy_rid, out IToyFactory? factory)) {
      if (spawned_toys.TryGetValue(factory!, out BaseToyHandler? handler)) {
        handler?.Destroy();
        factory?.Let(fac => spawned_toys.Remove(fac));

        return true;
      }
    }

    return false;
  }
}