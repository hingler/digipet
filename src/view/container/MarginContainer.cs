using System.Collections.Generic;
using System.Numerics;
using digipet.component;

namespace digipet.view.container;

public class MarginContainer : ViewComponent, IContainer {
  private readonly CompoundView cv = new();

  public int MarginPx = 4;

  public MarginContainer() : base() {
    cv.Anchor = new(0.0f, 0.0f);
  }

  public override IReadOnlyList<ViewComponent> GetChildren() => cv.GetChildren();
  public override void AddView(ViewComponent v) => cv.AddView(v);
  public override void RemoveView(ViewComponent v) => cv.AddView(v);

  public override void Draw(ICanvas canvas) {
    Vector2 margin_size = canvas.PxToRelative(MarginPx, MarginPx);
    cv.Size = Vector2.One - 2.0f * margin_size;
    cv.Offset = margin_size;
  }
}