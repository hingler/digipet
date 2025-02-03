using digipet.framework;
using digipet.image;
using digipet.sim.toy.handlers;
using digipet.sprite.attrib;

namespace digipet.sim.toy.factories;

#nullable enable

public class BallFactory : IToyFactory {
  public int RID => (int)SpriteID.OFFSET_TOY + 1;
  public string Name => "Ball";
  public string Description => "It's spherical.";
  public int StorePrice => 800;

  public ISprite? SpriteOverride { get; }

  public BallFactory(IEngine engine) {
    SpriteOverride = engine.GetDigipetAssetLoader().LoadSprite("sprites/toy/ball.png");
  }

  public BaseToyHandler Create(IEngine engine) {
    return new ToyBall(engine, this);
  }
}