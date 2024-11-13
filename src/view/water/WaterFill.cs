using System;
using System.Numerics;
using digipet.canvas.font;
using digipet.component;
using digipet.framework;
using digipet.input;
using digipet.sim.water;
using digipet.sprite.attrib;
using digipet.util;
using digipet.view.container;

namespace digipet.view.water;

public class WaterFill : ViewComponent {
  private readonly BorderContainer container;

  private readonly CompoundView visuals;

  private readonly SpriteView spout;
  private readonly SpriteView bowl;
  private readonly SpriteView stream;
  private readonly SpriteView cursor;

  private readonly IWaterSource source;

  private static readonly string STRING_PCT = "%";
  private static readonly string FILL_TXT = "Fill";
  private static readonly string STOPFILL_TXT = "Stop";

  private readonly SimpleTicker ticker;


  private bool isFilling = false;

  public WaterFill(
    IEngine engine,
    IWaterSource source
  ) {
    ISpriteFetcher fetcher = engine.GetSpriteFetcher();
    container = new();
    visuals = new();

    spout = new SpriteView(fetcher.GetSprite(SpriteID.WATER_SPOUT));
    bowl = new SpriteView(fetcher.GetSprite(SpriteID.WATER_BOWL));
    stream = new SpriteView(fetcher.GetSprite(SpriteID.WATER_STREAM));

    cursor = new SpriteView(fetcher.GetSprite(SpriteID.ICON_SELECTOR));

    this.source = source;

    ticker = new(0.5);

    ArrangeViews();
  }

  private void ArrangeViews() {
    visuals.SizePx = new(16.0f, 32.0f);

    visuals.AddView(spout);
    visuals.AddView(bowl);
    visuals.AddView(stream);

    spout.Anchor = new(0.0f, 0.0f);
    spout.Offset = new(0.0f, 0.0f);

    bowl.Anchor = new(0.0f, 1.0f);
    bowl.OffsetPx = new(0.0f, 31.0f);

    stream.Anchor = new(0.5f, 0.0f);
    stream.OffsetPx = new(8.0f, 9.0f);
    stream.Opacity = 0.0f;

    bowl.ZIndex = 1;
    spout.ZIndex = 1;
    stream.ZIndex = 0;

    visuals.Anchor = new(0.0f, 1.0f);
    visuals.Offset = new(0.0f, 1.0f);

    cursor.Anchor = new(1.0f, 0.5f);

    container.AddView(visuals);
    
    AddView(container);
    AddView(cursor);

    container.ZIndex = -2;
  }

  private static readonly double FILL_RATE = 0.2;

  public override bool HandleInput(InputType input, InputState state) {
    if (base.HandleInput(input, state)) {
      return true;
    }

    if (state != InputState.RELEASE) {
      switch (input) {
        case InputType.CONFIRM:
          isFilling = !isFilling;
          stream.Opacity = isFilling ? 1.0f : 0.0f;
          ticker.Reset();
          break;
        case InputType.BACK:
          PopSelf();
          break;
      } 
    }

    return true;
  }

  public override void Tick(double delta) {
    base.Tick(delta);
    if (isFilling) {
      source.Fill(FILL_RATE * delta);
      if (ticker.Update(delta)) {
        stream.PixelY = ticker.GetFrameCount() % 2 == 0 ? 9.0f : 11.0f;
      }
    }
  }

  public override void Draw(ICanvas canvas) {
    base.Draw(canvas);

    string val = ((int)Math.Ceiling(source.Contents / source.Capacity * 100.0)).ToString();
    Vector2 pctSize = canvas.GetStringSize(STRING_PCT, FontType.TINY, 1.0f);
    Vector2 px_size = canvas.GetPixelDims();

    float baseline = px_size.Y * ((int)FontType.EXTRA_LARGE + 10);

    Vector2 text_base = new(1.0f - px_size.X * 4.0f, baseline);
    Vector2 num_origin = new(1.0f - pctSize.X - px_size.X * 4.0f, baseline);

    canvas.Text(text_base, STRING_PCT, 1.0f, FontType.TINY, HorizontalAlign.RIGHT, Vector4.UnitW, 0);
    canvas.Text(num_origin, val, 1.0f, FontType.EXTRA_LARGE, HorizontalAlign.RIGHT, Vector4.UnitW, 0);

    Vector2 fill_origin = new(1.0f - 6 * px_size.X, 1.0f - 16 * px_size.Y);

    cursor.Offset = new(0.65f, fill_origin.Y);
    canvas.Text(fill_origin, isFilling ? STOPFILL_TXT : FILL_TXT, 1.0f, FontType.TINY, HorizontalAlign.RIGHT, Vector4.UnitW, 0);

    Vector2 fill_size = canvas.GetStringSize(STOPFILL_TXT, FontType.TINY, 1.0f) * canvas.GetSizePx();

    cursor.PixelX = MathF.Floor(canvas.GetSizePx().X - fill_size.X - 10);
    cursor.PixelY = MathF.Ceiling(fill_origin.Y * canvas.GetSizePx().Y - fill_size.Y / 2) + 1.5f;

  }
}