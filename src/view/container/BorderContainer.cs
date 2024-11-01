using System.Collections.Generic;
using System.Numerics;
using digipet.component;

namespace digipet.view.container;

public class BorderContainer : ViewComponent, IContainer {
  private readonly CompoundView contents = new();

  static private readonly int MARGIN_SIZE = 3;
  public BorderContainer() {}

  public void AddView(ViewComponent v) {
    contents.AddView(v);
  }

  public void RemoveView(ViewComponent v) {
    contents.RemoveView(v);
  }

  public IReadOnlyList<ViewComponent> GetComponents() {
    return GetChildren();
  }

  public override IReadOnlyList<ViewComponent> GetChildren() {
    return [contents];
  }

  public override void Draw(ICanvas canvas) {
    base.Draw(canvas);

    contents.SizePx = canvas.GetSizePx() - new Vector2(2.0f * MARGIN_SIZE);
    contents.OffsetPx = new(MARGIN_SIZE);
    
    Vector2 pixel_size = canvas.GetPixelDims();
    Vector4 color_gray = new(0.5f, 0.5f, 0.5f, 1.0f);

    // generify this for other windows
    // (tba: alpha dither?)

    canvas.Rect(Vector2.Zero, Vector2.One, 4, color_gray);
    canvas.Rect(pixel_size, Vector2.One - pixel_size, 3, new(0, 0, 0, 1));
    canvas.Rect(2.0f * pixel_size, Vector2.One - 2.0f * pixel_size, 2, color_gray);
    canvas.Rect(3.0f * pixel_size, Vector2.One - 3.0f * pixel_size, 1, Vector4.One);
  }
}