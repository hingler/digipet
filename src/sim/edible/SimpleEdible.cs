using digipet.image;

namespace digipet.sim.edible;

public class SimpleEdible : IEdiblePickup {
  public int RID { get; set; } = 0;
  public string Name { get; set; } = "UNKNOWN";
  public string Description { get; set; } = "N/A";
  public int StorePrice { get; set; } = 1;
  public int Rarity { get; set; } = 1;
  public double Satiability { get; set; } = 0.5;
  public double Appeal { get; set; } = 0.0;
  public double Variance { get; set; } = 1.0;

  // - sprite
  // - name of this item
  // - description? probably
  // - store price
  // - identifier? same one used for sprite i'm thinking
  //   (enough so that we can shuffle around a preference list)

  public SimpleEdible() {}

  public double GetFill() {
    return Satiability;
  }
}