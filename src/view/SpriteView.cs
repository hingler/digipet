using System.Numerics;
using System.Reflection;
using digidev.impl;
using digipet.component;
using digipet.image;

namespace digipet.view;

public class SpriteView(ISprite sprite) : ViewComponent {
  public ISprite sprite = sprite;
  public bool tile = false;

  public Vector2 Dims => sprite?.Dims ?? Vector2.Zero;
  public SpriteView() : this(null) {}

  public override void Draw(ICanvas canvas) {
    base.Draw(canvas);
    canvas.Tex(sprite, Vector2.Zero, Vector2.One, tile);
  }
}