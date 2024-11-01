using System.Numerics;
using digipet.canvas.font;
using digipet.component;
using digipet.image;

namespace digipet.view.sky;

public class SkyCounterMini : ViewComponent {
  private readonly Text counter;
  private readonly SpriteView icon;

  private readonly CompoundView container;

  public string Content {
    get => counter.Content;
    set => counter.Content = value;
  }

  public SkyCounterMini(
    ISprite sprite
  ) {
    container = new();

    counter = new() {
      Font = FontType.TINY,
      Scale = 2.0f,
      Offset = new(1.0f, 1.0f),
      Alignment = HorizontalAlign.RIGHT,
      Color = Vector4.One,
      Content = "asdasd"
    };

    icon = new() {
      sprite = sprite,
      Anchor = new(0.0f, 1.0f),
      Offset = new(0.0f, 1.0f),
      SizePx = new(12, 12)
    };

    container.AddView(icon);
    container.AddView(counter);
  }

  public override void Tick(double delta) {
    base.Tick(delta);
  }

    public override void Draw(ICanvas canvas) {
      base.Draw(canvas);
      container.PreDraw(canvas);
    }
}