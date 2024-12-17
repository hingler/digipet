using System.Numerics;
using System.Reflection;
using digipet.component;
using digipet.image;
using digipet.util;

namespace digipet.view;

#nullable enable

public class SpriteView : ViewComponent {
  // dep
  public ISprite? sprite;
  public float Scale = 1.0f;

  private static readonly Vector2 HALF = new(0.5f);

  public ISprite? Sprite {
    get => sprite;
    set {
      sprite = value;
      SizePx = Dims;
    }
  }
  public bool Tile = false;

  public Vector2 Dims => sprite?.Dims ?? Vector2.Zero;
  public SpriteView() : this(null) {}
  public SpriteView(ISprite? sprite) {
    Sprite = sprite;
  }

  public override void Draw(ICanvas canvas) {
    base.Draw(canvas);
    Vector2 start = HALF - (Scale * HALF);
    Vector2 end = HALF + (Scale * HALF);
    sprite?.Let(s => {
      canvas.Tex(s, start, end, Tile);
    });
  }
}