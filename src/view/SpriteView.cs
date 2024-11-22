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
    canvas.Tex(sprite, Vector2.Zero, Vector2.One, Tile);
  }
}