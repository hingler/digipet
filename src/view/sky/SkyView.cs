using System;
using System.Collections.Generic;
using System.Numerics;
using digipet.component;
using digipet.sprite.attrib;

namespace digipet.view.sky;

public class SkyView : ViewComponent {
  // draw sky along bottom

  private readonly IList<SpriteView> sky_sprites = [];

  private double dt = 0.0;
  private int frame_count = 0;
  private const double frame_time = 0.25;

  private const int sky_offset_px = 5;

  public double dt_acc = 0.0;
  public double DepthOffset = 0.0;

  public SkyView(
    ISpriteFetcher fetcher
  ) {
    sky_sprites.Add(new SpriteView(fetcher.GetSprite(SpriteID.SKY_FRONT)));
    sky_sprites.Add(new SpriteView(fetcher.GetSprite(SpriteID.SKY_MID)));
    sky_sprites.Add(new SpriteView(fetcher.GetSprite(SpriteID.SKY_BACK)));

    foreach (SpriteView sprite in sky_sprites) {
      sprite.Anchor = new(0.0f, 1.0f);
      sprite.tile = true;
    }
  }

  public override void Tick(double delta) {
    base.Tick(delta);

    dt += delta;
    dt_acc += delta;
    if (dt > frame_time) {
      dt -= frame_time;
      frame_count++;
    }
  }

  public override void Draw(ICanvas canvas) {
    base.Draw(canvas);
    Vector2 canvas_px = canvas.GetSizePx();
    Vector2 sprite_dims = canvas.PxToRelative((int)canvas_px.X + 28, 1);

    // canvas.Rect(Vector2.Zero, Vector2.One, 0, Vector4.UnitW);

    // position

    for (int i = 0; i < sky_sprites.Count; i++) {
      SpriteView sprite = sky_sprites[i];

      sprite.Size = new(
        sprite_dims.X,
        sprite_dims.Y * sprite.Dims.Y
      );

      double offset_depth = DepthOffset * (3 - i);

      
      sprite.Offset = canvas.PxToRelative(
        (((frame_count + i) / (1 << (i))) % 14) - 14,
        (int)(canvas_px.Y - sky_offset_px - offset_depth)
      );
    }

    for (int i = sky_sprites.Count - 1; i >= 0; --i) {
      int floor_start_px = (int)(canvas_px.Y - sky_offset_px - DepthOffset * (3 - i) - 4);
      Vector2 floor_start_local = canvas.PxToRelative(0, floor_start_px);
      canvas.Rect(floor_start_local, Vector2.One, 0, Vector4.One);
      sky_sprites[i].PreDraw(canvas);
      
    }
  }
}