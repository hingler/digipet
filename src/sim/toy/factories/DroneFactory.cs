using digipet.framework;
using digipet.image;
using digipet.sim.toy.handlers;
using digipet.sprite.attrib;

namespace digipet.sim.toy.factories;

#nullable enable

public class DroneFactory : IToyFactory {
  public int RID => (int)SpriteID.OFFSET_TOY + 2;
  public string Name => "Drone";
  public string Description => "Up to 5 miles of range!";
  public int StorePrice => 3200;

  public ISprite? SpriteOverride { get; }

  public DroneFactory(IEngine engine) {
    // register w manager
    IAnimatedSprite sprite = engine.GetDigipetAssetLoader().LoadAnimatedSprite("sprites/toy/drone_sprites.png");
    sprite.HFrames = 3;
    sprite.VFrames = 1;
    sprite.Frame = 0;

    SpriteOverride = sprite;
  }

  public BaseToyHandler Create(IEngine engine) {
    return new ToyDrone(engine, this);
  }
}