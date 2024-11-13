using digipet.sim.water;
using digipet.sprite.attrib;

namespace digipet.world.fixture;

public class SimpleWaterFixture(IWaterSource source) : IFixture {
  public int RID => (int)SpriteID.WATER_BOWL;
  public string Name => "watersource";
  public string Description => "water bowl";
  public int StorePrice => 0;
  public int Rarity => 1;

  public readonly IWaterSource source = source;
}