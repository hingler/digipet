using System.Numerics;
using digipet.component;

namespace digipet.view.bg;

public class ColorRect(Vector4 col) : ViewComponent {

  public override void Draw(ICanvas canvas) {
    base.Draw(canvas);
    canvas.Rect(Vector2.Zero, Vector2.One, 0, col, 0);
  }
}