using System.Numerics;
using digipet.file;
using digipet.framework;
using digipet.image;
using digipet.rpg;
using digipet.sprite;
using digipet.util;

namespace digipet.view.rpg;

public class SelectorEntity : IWorldEntity {
  public Vector2 Position => lerper.Cursor;
  private readonly Vec2Lerper lerper;
  public Vector2 Velocity => Vector2.Zero;

  public Vector2 Target {
    get => lerper.Target;
    set => lerper.Target = value;
  }

  public bool Active => true;

  private readonly IAnimatedSprite cursor;
  public ISprite Sprite => cursor;

  public Vector2 WorldDims => Sprite.Dims * SpriteScale;

  public Vector2 SpriteScale { get; set; } = Vector2.One;

  private readonly FrameTicker ticker;
  public SelectorEntity(IEngine engine) {
    lerper = new(10.0);

    IFileLoader loader = engine.GetDigipetAssetLoader();
    cursor = loader.LoadAnimatedSprite("sprites/rpg/overworld/selector.png");
    cursor.HFrames = 2;
    cursor.VFrames = 1;

    ticker = new(0.5, cursor.GetFrameCount());
  }

  public void Tick(double delta) {
    lerper.Tick(delta);
    if (ticker.Tick(delta)) {
      cursor.Frame = ticker.Frame;
    }
  }

  public void Reset() {
    lerper.Reset();
  }
}