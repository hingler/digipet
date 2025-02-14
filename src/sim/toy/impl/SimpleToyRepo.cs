using digipet.framework;
using digipet.sim.db;
using digipet.sim.toy.factories;

namespace digipet.sim.toy.impl;

#nullable enable

public class SimpleToyRepo : ISimRepo<IToyFactory> {
  private readonly DataRepo<IToyFactory> factories = new();

  public SimpleToyRepo(IEngine engine) {
    // for now: just build inline hehehe
    factories.AddItem(new CarFactory(engine));
    factories.AddItem(new BallFactory(engine));
    factories.AddItem(new DroneFactory(engine));
  }

  public IReadOnlyCollection<IToyFactory> GetEntries() => factories.GetEntries();
  public IReadOnlyCollection<int> GetDescriptors() => factories.GetDescriptors();
  public IToyFactory Fetch(int descriptor) => factories.Fetch(descriptor);
  public bool TryFetch(int descriptor, out IToyFactory? output) => factories.TryFetch(descriptor, out output);
}