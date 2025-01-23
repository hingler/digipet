using digipet.image;

namespace digipet.sim.edible;

#nullable enable

public interface IEdiblePickup : IWorldItem, IPurchasable {
  // affects prob of appealing in store
  public int Rarity { get; }

  public double Appeal { get; }
  public double Variance { get; }

  // thinking: separate these out, and let the pet content handle it
  // rid -> ediblepickup -> backend logic

  double GetFill();
  // don't like that the foods themselves are responsible for this?
}