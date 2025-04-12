// keep it simple
// draw a single sprite, optionally tile

// (if tile: stretch - else, just place rel to center)
// (z-divide: treat "1" as the fg and scroll by "1.0 / Z")

using System.Numerics;
using digipet.component;
using digipet.image;
using digipet.util;
using digipet.view.rpg;
using static digipet.util.DigiMath;

namespace digipet.view.world;

#nullable enable

public class ParallaxBGView : ViewComponent, IPseudoDepthDisplay {

  float z_dist = 1.0f;
  public float ZDist {
    get => z_dist;
    set {
      z_dist = MathF.Max(value, 0.00001f);
      // -1 -> 0
      ZIndex = -z_dist + 1;
    }
  }

  public bool LockX { get; set; } = false;
  public bool LockY { get; set; } = true;

  public Vector2 WorldOrigin { get; set; }

  float world_scale_ = 1.0f;
  public float WorldScale { 
    get => world_scale_;
    set {
      world_scale_ = value;
      ResizeSprite();
    }
  }

  private float scale_ = 1.0f;
  public float SpriteScale {
    get => scale_;
    set {
      scale_ = value;
      ResizeSprite();
    }
  }

  public Vector2 BGOffset { get; set; } = Vector2.Zero;
  public ISprite? Sprite {
    get => view.Sprite;
    set {
      view.Sprite = value;
      ResizeSprite();
    }
  }

  private static readonly ILogger logger = LoggerSingleton.GetStaticLogger<ParallaxBGView>();

  private bool tile_x = false;
  private bool tile_y = false;

  public bool TileX {
    get => tile_x;
    set {
      tile_x = value;
      view.Tile = true;
      ResizeSprite();
    }
  }

  public bool TileY {
    get => tile_y;
    set {
      tile_y = value;
      view.Tile = true;
      ResizeSprite();
    }
  }



  private readonly SpriteView view;

  public ParallaxBGView() {
    view = new() {
      Anchor = new(0.5f, 0.5f),
      Offset = new(0.5f, 0.5f),
      Tile = true
    };

    // how do we deal with spriteview being a certain size?
    AddView(view);

    ResizeSprite();
  }

  public ViewComponent GetRootView() => this;

  private void ResizeSprite() {
    float scale_fac = (SpriteScale / WorldScale);

    float size_x = TileX ? SizePx.X * 2 : (Sprite?.Dims.X ?? 1) * scale_fac;
    float size_y = TileY ? SizePx.Y * 2 : (Sprite?.Dims.Y ?? 1) * scale_fac;
    view.SizePx = new(size_x, size_y);
  }

  public override void Reflow(ICanvas canvas) {
    base.Reflow(canvas);
    ResizeSprite();
  }

  public override void Draw(ICanvas canvas) {
    base.Draw(canvas);
    // gets position in XY world space
    Vector2 zdist = new(LockX ? 1.0f : ZDist, LockY ? 1.0f : ZDist);
    Vector2 shift = BGOffset / zdist;

    float world_tile_width = SpriteScale * Sprite?.Dims.X ?? 1.0f;

    // this is where the center of our sprite should be in absolute XY space
    Vector2 origin_dist = WorldOrigin - (WorldOrigin / zdist) + shift;

    // dist from parallax'd origin to world origin
    Vector2 world_delta = origin_dist - WorldOrigin;
    // mod the distance, as any number of tile-sized steps are equivalent
    world_delta.X %= world_tile_width;
    
    // correct the origin st we refer to a "nearby" offset, instead of the origin offset
    Vector2 nearest_origin_dist = WorldOrigin + world_delta;

    // width of a pixel, in world units
    float scale = SpriteScale / WorldScale;

    Vector2 screen_coord = this.ProjectRelative(nearest_origin_dist);
    view.Offset = screen_coord;
    view.Scale = scale;
  }


}