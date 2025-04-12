using System.Numerics;
using digipet.component;
using digipet.image;
using digipet.view.rpg;

namespace digipet.view.world;

#nullable enable

// like parallax, but doesn't move
public class StaticBGView : ViewComponent, IPseudoDepthDisplay {
  float z_dist = 1.0f;
  public float ZDist {
    get => z_dist;
    set {
      z_dist = MathF.Max(value, 0.00001f);
      ZIndex = -z_dist;
    }
  }

  public bool LockX { get; set; }
  public bool LockY { get; set; }

  public Vector2 WorldOrigin { get; set; }
  public float WorldScale { get; set; }
  public ISprite? Sprite {
    get => view.Sprite;
    set => view.Sprite = value;
  }

  // this should be the world-size of individual pixels
  public float SpriteScale { get; set; } = 1.0f;

  public Vector2 BGOffset = Vector2.Zero;
  public bool Tile {
    get => view.Tile;
    set => view.Tile = value;
  }

  private readonly SpriteView view;

  public StaticBGView() {
    view = new() {
      Anchor = new(0.5f, 0.5f),
      Size = new(1.0f, 1.0f),
      Offset = new(0.5f, 0.5f),
      Tile = false
    };

    AddView(view);
  }

  public ViewComponent GetRootView() => this;

  public override void Draw(ICanvas canvas) {

    // start from global origin, 0, and move based on scale factor
    view.Offset = Vector2.One * 0.5f + (BGOffset * WorldScale);
    view.Scale = SpriteScale / WorldScale;
  }
}