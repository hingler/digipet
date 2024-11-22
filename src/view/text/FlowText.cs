// what should the idea be with this?

// - specify a size for text
// - enforce word breaks

// - add a word at a time to the string
// - once string violates a dimension, add a line break
// - thinking: box in, fill the surrounding area with text

using System.Numerics;
using digipet.canvas.font;
using digipet.component;
using digipet.framework;
using digipet.util;

namespace digipet.view.text;

public class FlowText : ViewComponent {
  private readonly ITextFlow flow;

  public string Content {
    get => flow.Content;
    set => flow.Content = value;
  }

  public FontType Font {
    get => flow.Font;
    set => flow.Font = value;
  }

  public float DisplayedLineOffset = 0.0f;

  public FlowText(IEngine engine) : this(new TextFlowHandler(engine)) {}

  public FlowText(ITextFlow flow) {
    this.flow = flow;
  }

  public override void Reflow(ICanvas canvas) {
    base.Reflow(canvas);
    flow.Bounds = SizePx;
  }

  public override void Draw(ICanvas canvas) {
    base.Draw(canvas);
    IReadOnlyList<string> lines = flow.GetLines();
    for (int i = 0; i < lines.Count; i++) {
      Vector2 baseline_px = new(0.1f, flow.GetTextBaseline(i, DisplayedLineOffset));
      Vector2 baseline_canvas = canvas.PxToRelative(baseline_px);
      canvas.Text(baseline_canvas, lines[i], 1.0f, Font, HorizontalAlign.LEFT, DigiColor.BLACK, 0);
    }
  }
}