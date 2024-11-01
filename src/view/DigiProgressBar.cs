using System;
using System.Numerics;
using digipet.component;

namespace digipet.view;

public class DigiProgressBar : ViewComponent {
  // bar + edges
  private float _fill;
  public float Fill {
    get => _fill;
    set => _fill = MathF.Min(MathF.Max(value, 0.0f), 1.0f);
  }
  public Vector4 color = Vector4.UnitW;

  public DigiProgressBar() {}

  public override void Draw(ICanvas canvas) {
    Vector2 px = canvas.GetPixelDims();
    Vector2 half_px = px / 2.0f;

    Vector2 bl = new(half_px.X, 1.0f - half_px.Y);
    Vector2 br = Vector2.One - half_px;

    // bounds
    canvas.Line(
      Vector2.Zero + half_px,
      bl,
      1.0f,
      Vector4.UnitW
    );

    canvas.Line(
      new Vector2(1.0f - half_px.X, half_px.Y),
      br,
      1.0f,
      Vector4.UnitW
    );

    canvas.Line(
      bl,
      br,
      1.0f,
      Vector4.UnitW
    );

    // the bar itself
    Vector2 rect_start = new(
      0.0f + px.X,
      0.0f + 2.0f * px.Y
    );

    Vector2 rect_end = new(
      Fill * (1.0f - 2.0f * px.X) + px.X,
      1.0f - (2.0f * px.Y)
    );

    canvas.Rect(
      rect_start, rect_end, 0, Vector4.UnitW
    );
  }
}