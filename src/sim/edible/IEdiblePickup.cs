using digipet.image;

namespace digipet.sim.edible;

public interface IEdiblePickup : IWorldItem {
  public string Name { get; }
  public string Description { get; }
  public int StorePrice { get; }
  public int Rarity { get; }
}