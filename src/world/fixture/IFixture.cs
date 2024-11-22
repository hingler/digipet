using digipet.sim;

namespace digipet.world.fixture;

// furniture item
public interface IFixture : IWorldItem {
  public int Rarity { get; }
}