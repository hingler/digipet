using System.Numerics;
using digipet.component;
using digipet.util;
using digipet.view.rpg;
using LinePair = System.Tuple<System.Numerics.Vector2, System.Numerics.Vector2, digipet.util.DigiColor, float>;

namespace digipet.view.world;


#nullable enable

public class LineView : ViewComponent, IRPGDisplay {
  private readonly List<LinePair> lines;

  public Vector2 WorldOrigin { get; set; } = Vector2.Zero;
  public float WorldScale { get; set; } = 1.0f;

  public LineView() {
    lines = [];
  }

  public void AddLine(Vector2 start, Vector2 end, DigiColor color, float width = 1.0f) {
    lines.Add(new(start, end, color, width));
  }

  public ViewComponent GetRootView() => this;
  public override void Draw(ICanvas canvas) {
    base.Draw(canvas);

    foreach (LinePair lp in lines) {
      Vector2 start_rel = this.ProjectRelative(lp.Item1);
      Vector2 end_rel = this.ProjectRelative(lp.Item2);

      canvas.Line(start_rel, end_rel, lp.Item4, lp.Item3);
    }
  }
}