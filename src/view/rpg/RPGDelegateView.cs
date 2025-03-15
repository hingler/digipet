using System.Numerics;
using digipet.component;
using digipet.framework;

namespace digipet.view.rpg;

public class RPGDelegateView(IEngine engine) : ViewComponent(), IRPGDisplay {
  private readonly RPGDisplayDelegate underlying = new(engine);

  public Vector2 WorldOrigin {
    get => underlying.WorldOrigin;
    set => underlying.WorldOrigin = value;
  }

  public float WorldScale {
    get => underlying.WorldScale;
    set => underlying.WorldScale = value;
  }

  public ViewComponent GetRootView() => this;

  public override void Reflow(ICanvas canvas) {
    base.Reflow(canvas);
    underlying.SizePx = SizePx;
  }

  public override void AddView(ViewComponent view) {
    if (view is IRPGDisplay display) {
      AddDisplay(display);
    } else {
      base.AddView(view);
    }
  }

  public void AddDisplay(IRPGDisplay display) {
    ViewComponent root = display.GetRootView();
    base.AddView(root);
    root.Size = Vector2.One;
    root.Anchor = Vector2.Zero;
    root.Offset = Vector2.Zero;

    underlying.AddDisplay(display);
  }
}