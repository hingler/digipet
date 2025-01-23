namespace digipet.sim;

public interface IPurchasable : IWorldItem {
  string Name { get; }
  string Description { get; }
  int StorePrice { get; }
}