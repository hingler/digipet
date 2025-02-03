using digipet.framework;
using digipet.image;
using digipet.sim.toy.handlers;
using digipet.sprite.attrib;

namespace digipet.sim.toy.factories;

#nullable enable

public class CarFactory : IToyFactory {
  public int RID => (int)SpriteID.OFFSET_TOY;
  public string Name => "Car";
  public string Description => "Perfect for smashing into your wall.";
  public int StorePrice => 1500;

  public ISprite? SpriteOverride { get; }

  public CarFactory(IEngine engine) {
    // register w manager
    SpriteOverride = engine.GetDigipetAssetLoader().LoadSprite("sprites/toy/car.png");
  }

  public BaseToyHandler Create(IEngine engine) {
    return new ToyCar(engine, this);
  }
}