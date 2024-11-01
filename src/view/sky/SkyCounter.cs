using System;
using System.Numerics;
using digipet.canvas.font;
using digipet.component;
using digipet.image;
using digipet.util;

namespace digipet.view.sky;

public class SkyCounter : ViewComponent {
  private readonly CompoundView compoundView;
  private readonly Text counter;

  private readonly SpriteView icon;

  private const int MARGIN_PX = -4;
  public string Content {
    get => counter.Content;
    set => counter.Content = value;
  }

  public int ScaleFactor {
    get => (int)Math.Round(counter.Scale);
    set => counter.Scale = value;
  }

  public SkyCounter(
    ISprite sprite,
    FontType font
  ) {
    compoundView = new();

    counter = new() {
      Font = font,
      Scale = 1.0f,
      Offset = new(0.5f, 1.0f),
      Alignment = HorizontalAlign.CENTER,
      Color = Vector4.One,
      Content = "aasfdfsgdfsg"
    };

    icon = new() {
      sprite = sprite,
      Anchor = new(0.5f, 0.0f),
      Offset = new(0.5f, 0.0f)
    };

    compoundView.AddView(icon);
    compoundView.AddView(counter);
  }

  public override void Tick(double delta) {
    base.Tick(delta);
    // nothing else rly
  }

  public override void Draw(ICanvas canvas) {
    base.Draw(canvas);

    // figure out how much space our text takes up
    Vector2 text_space = canvas.GetStringSize(
      counter
    );

    float text_height = text_space.Y + canvas.PxToRelative(0, MARGIN_PX).Y;
    float icon_height = MathF.Min(text_space.Y * 0.75f, 1.0f - text_height);


    Vector2 size_px = canvas.GetSizePx();
    Vector2 icon_size = canvas.PxToRelative(new(size_px.Y * icon_height));
    icon.Size = icon_size;

    float net_height = icon_size.Y + text_height;

    icon.Offset = new(0.5f, 0.5f - (net_height / 2.0f));
    counter.Offset = new(0.5f, 0.5f + net_height / 2.0f);

    compoundView.PreDraw(canvas);
  }
}