using System.Numerics;
using System.Runtime.Intrinsics;
using digipet.component;

namespace digipet.view.container;

public class BevelContainer : ViewComponent, IContainer {

  private readonly CompoundView contents = new();

  public float BevelSize = 1.0f;

  public float Depth = 0.5f;

  private static readonly Vector3 COL_LIGHT = new(1.0f, 1.0f, 1.0f);
  private static readonly Vector3 COL_DARK = new(0.0f, 0.0f, 0.0f);
  public BevelContainer() : base() {}

  public override void AddView(ViewComponent v) => contents.AddView(v);
  public override void RemoveView(ViewComponent v) => contents.RemoveView(v);
  public override IReadOnlyList<ViewComponent> GetChildren() {
    return [ contents ];
  }

  public override void Reflow(ICanvas canvas) {
    contents.Offset = Vector2.Zero;
    contents.Size = Vector2.One;
  }

  public override void Draw(ICanvas canvas) {
    base.Draw(canvas);

    Vector4 col_tl = new(Depth > 0.0f ? COL_LIGHT : COL_DARK, Math.Abs(Depth));
    Vector4 col_br = new(Depth > 0.0f ? COL_DARK : COL_LIGHT, Math.Abs(Depth));
    
    Vector2 half_px = (canvas.GetPixelDims() / 2.0f) * BevelSize;

    Vector2 tl = new(-half_px.X, -half_px.Y);
    Vector2 tr = new(1.0f + half_px.X, -half_px.Y);
    Vector2 bl = new(-half_px.X, 1.0f + half_px.Y);
    Vector2 br = new(1.0f + half_px.X, 1.0f + half_px.Y);


    canvas.Line(
      tl, 
      tr, 
      BevelSize, 
      col_tl
    );

    canvas.Line(
      tl, 
      bl, 
      BevelSize, 
      col_tl
    );

    canvas.Line(
      br, 
      tr, 
      BevelSize, 
      col_br
    );

    canvas.Line(
      br,
      bl,
      BevelSize,
      col_br
    );
  }
}