using digipet.sim;

namespace digipet.world.fixture;

// furniture item
public interface IFixture : IWorldItem {
  public string Name { get; }
  public string Description { get; }
  public int StorePrice { get; }
  public int Rarity { get; }
}