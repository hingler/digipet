using digipet.image;

namespace digipet.sim.edible;

public class SimpleEdible : IEdiblePickup {

  public ISprite Sprite { get; set; } = null;
  public string Name { get; set; } = "UNKNOWN";
  public string Description { get; set; } = "N/A";
  public int StorePrice { get; set; } = 1;
  public int Rarity { get; set; } = 1;

  // - sprite
  // - name of this item
  // - description? probably
  // - store price
  // - identifier? same one used for sprite i'm thinking
  //   (enough so that we can shuffle around a preference list)

  public SimpleEdible(
    ISprite sprite,
    string name,
    string description,
    int store_price
  ) {
    Sprite = sprite;
    Name = name;
    Description = description;
    StorePrice = store_price;
  }
}