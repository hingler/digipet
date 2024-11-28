using System.Numerics;
using digipet.canvas.font;
using digipet.framework;

namespace digipet.view.text;

// handle visible chars
// handle line advances
public class TextFlowAnimator : ITextFlow {
  private readonly TextFlowHandler flow_handler;

  public string Content {
    get => flow_handler.Content;
    set {
      flow_handler.Content = value;
      max_chars_ = GetCharCap();
    }
  }

  public Vector2 Bounds {
    get => flow_handler.Bounds;
    set {
      flow_handler.Bounds = value;
      max_chars_ = GetCharCap();
    }
  }

  public FontType Font {
    get => flow_handler.Font;
    set {
      flow_handler.Font = value;
      max_chars_ = GetCharCap();
    }
  }

  public int LineSpacing {
    get => flow_handler.LineSpacing;
    set {
      flow_handler.LineSpacing = value;
      max_chars_ = GetCharCap();
    }
  }

  private double visible_chars = 0.0;

  private int line_offset_;
  private int max_chars_;
  public int LineOffset {
    get => line_offset_;
    set {
      line_offset_ = value;
      max_chars_ = GetCharCap();
    }
  }
  public double AnimationSpeed = 30.0;

  public TextFlowAnimator(IEngine engine) {
    flow_handler = new(engine);
    LineOffset = 0;
  }

  public void Reset() {
    visible_chars = 0;
    LineOffset = 0;
  }

  // updates animator state
  public void Update(double delta) {
    visible_chars = Math.Min(
      visible_chars + (delta * AnimationSpeed),
      max_chars_
    );
  }

  public IReadOnlyList<string> GetLines() {
    IReadOnlyList<string> lines = flow_handler.GetLines();
    int char_buffer = (int)visible_chars;
    int line_cursor = 0;

    IList<string> line_output = [];

    while (line_cursor < LineOffset) {
      // deduct lines which should not be visible
      char_buffer -= lines[line_cursor++].Length;
    }

    while (char_buffer > 0 && line_cursor < lines.Count) {
      string line = lines[line_cursor++];
      if (line.Length > char_buffer) {
        // truncate
        line_output.Add(line[..char_buffer]);
        char_buffer = 0;
      } else {
        // add full line
        line_output.Add(line);
        char_buffer -= line.Length;
      }
    }

    return line_output.AsReadOnly();
  }

  public IReadOnlyList<int> GetLineCharOffsets() {
    IReadOnlyList<int> source = flow_handler.GetLineCharOffsets();
    IList<int> res = [];
    for (
      int i = line_offset_; 
      i < source.Count; 
      i++
    ) {
      res.Add(source[i]);
    }

    // extra data doesn't matter to me
    return res.AsReadOnly();
  }

  public string GetLinesAsString() {
    return string.Join('\n', GetLines());
  }

  public void AdvanceToNextStop() {
    visible_chars = max_chars_;
  }

  // true if waiting
  public bool Waiting() {
    return visible_chars == max_chars_;
  }

  // true if dialogue is complete
  public bool Complete() {
    return (visible_chars == max_chars_) && ((LineOffset + GetMaxLines()) >= flow_handler.GetLines().Count);
  }
  public float GetTextBaseline(int line_num, float desired_offset) {
    // desired offset: offset we want to apply to current position
    // line offset: number of lines we want to display

    // scroll then advance - what will that do?
    return flow_handler.GetTextBaseline(line_num, desired_offset - line_offset_);
  }

  public float GetLineHeight() {
    return flow_handler.GetLineHeight();
  }

  public int GetTotalLines() {
    return flow_handler.GetLines().Count;
  }

  public int GetMaxLinesVisible() {
    return Math.Min(flow_handler.GetMaxLinesVisible(), flow_handler.GetLines().Count - line_offset_);
  }

  public int GetMaxLines() {
    return GetMaxLinesVisible();
  }

  // create a test for this next!!
  // - animator exposes specifications of line height, etc...
  // - write a "dialogue view" that actually animates it

  private int GetCharCap() {
    IReadOnlyList<string> lines = flow_handler.GetLines();
    int max_lines = Math.Min(flow_handler.GetMaxLinesVisible() + LineOffset, lines.Count);
    int char_sum = 0;
    for (int i = 0; i < max_lines; i++) {
      char_sum += lines[i].Length;
    }

    return char_sum;
  }
}