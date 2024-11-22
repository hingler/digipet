using System.Collections.Generic;
using System.Numerics;
using digipet.component;

namespace digipet.view.container;

public class MarginContainer : ViewComponent, IContainer {
  private readonly CompoundView cv = new();

  private Vector2 margins_ = new(4.0f);

  public float MarginPx {
    get => Margins[0];
    set {
      Margins = new(value, value);
    }
  }

  public Vector2 Margins {
    get => margins_;
    set {
      margins_ = value;
      QueueReflow();
    }
  }

  public MarginContainer() : base() {
    cv.Anchor = new(0.0f, 0.0f);

    QueueReflow();
  }

  public override IReadOnlyList<ViewComponent> GetChildren() => [ cv ];
  public override void AddView(ViewComponent v) => cv.AddView(v);
  public override void RemoveView(ViewComponent v) => cv.RemoveView(v);

  public override void Reflow(ICanvas canvas) {
    Vector2 margin_size = canvas.PxToRelative(Margins.X, Margins.Y);
    cv.Size = Vector2.One - 2.0f * margin_size;
    cv.Offset = margin_size;
  }
}