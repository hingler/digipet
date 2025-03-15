using System.Numerics;
using digipet.component;
using digipet.util;

namespace digipet.view.bg;

public class ColorRect(Vector4 col) : ViewComponent {
  public ColorRect() : this(Vector4.One) {}
  public DigiColor Color = col;
  public override void Draw(ICanvas canvas) {
    base.Draw(canvas);
    canvas.Rect(Vector2.Zero, Vector2.One, 0, Color, 0);
  }
}