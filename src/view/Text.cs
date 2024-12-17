using System.Numerics;
using digipet.canvas.font;
using digipet.component;
using digipet.util;

namespace digipet.view;

public class Text : ViewComponent {
  public string Content = "";
  public float Scale = 1.0f;
  public HorizontalAlign Alignment = HorizontalAlign.LEFT;
  public FontType Font = FontType.TINY;
  public Vector4 Color = Vector4.One;

  public Text() : base() {}

  public override void Draw(ICanvas canvas) {
    base.Draw(canvas);
    canvas.Text(Anchor, Content, Scale, Font, Alignment, Color);
  }
}