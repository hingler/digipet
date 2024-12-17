using System.Numerics;
using digipet.canvas.font;
using digipet.framework;
using digipet.util;

namespace digipet.view.text;

public class TextFlowHandler : ITextFlow {
  private string content_ = "";
  public string Content {
    get => content_;
    set {
      // tba: optimize reflow
      // thinking: check for our lines inside the new "content" val
      string new_string = value.ReplaceLineEndings();

      // this is costly i think
      int first_deviant_line = GetFirstChangedLine(new_string);

      content_ = value;
      ReflowLines(first_deviant_line - 1);
    }
  }

  private Vector2 bounds_ = Vector2.Zero;

  // bounds, in px
  public Vector2 Bounds {
    get => bounds_;
    set {
      bounds_ = value;
      ReflowLines();
    }
  }

  private FontType font_ = FontType.TINY;
  public FontType Font {
    get => font_;
    set {
      font_ = value;
      ReflowLines();
    }
  }

  private int line_space_ = 0;
  public int LineSpacing {
    get => line_space_;
    set {
      line_space_ = value;
      ReflowLines();
    }
  }

  private readonly IFontHelper helper;

  private IList<string> lines;
  private IList<int> line_starts;

  public TextFlowHandler(IEngine engine) {
    helper = engine.GetFontHelper();
    lines = [];
    line_starts = [];
  }

  public IReadOnlyList<string> GetLines() {
    if (lines.Count <= 0 && Content.Length > 0) {
      ReflowLines();
    }

    return lines.AsReadOnly();
  }

  public IReadOnlyList<int> GetLineCharOffsets() {
    return line_starts.AsReadOnly();
  }

  public string GetLinesAsString() {
    return string.Join('\n', GetLines());
  }

  public float GetTextBaseline(int line_num, float desired_offset) {
    float text_height = GetLineHeight() * GetMaxLinesVisible_Absolute();

    // wiggle room with max lines
    float text_bounds_delta = Bounds.Y - text_height;

    float baseline_delta = FontSizeHandler.GetFontAscent(Font);
    float subsequent_lines = GetLineHeight() * (line_num - desired_offset);

    return baseline_delta + subsequent_lines + (text_bounds_delta / 2.0f);
  }

  public float GetLineHeight() {
    return FontSizeHandler.GetFontAscent(Font) + FontSizeHandler.GetFontDescent(Font) + LineSpacing;
  }

  private int GetMaxLinesVisible_Absolute() {
    return (int)Math.Floor(Bounds.Y / GetLineHeight());
  }

  public int GetMaxLinesVisible() {
    int max_lines = GetMaxLinesVisible_Absolute();
    return Math.Min(max_lines, GetLines().Count);
  }

  private int GetFirstChangedChar(string new_string) {
    int chars_max = Math.Min(content_.Length, new_string.Length);
    int cursor = 0;
    while (cursor < chars_max && (content_[cursor] == new_string[cursor])) {
      cursor++;
    }

    return cursor;
  }

  private int GetFirstChangedLine(string new_string) {
    int deviant_char = GetFirstChangedChar(new_string);

    for (int i = 0; i < (lines.Count - 1); i++) {
      if (line_starts[i + 1] > deviant_char) {
        // change on line *i*
        // conservatively roll back to prev line
        return i;
      }
    }

    // conservatively reparse the last TWO lines
    return lines.Count - 1;
  }

  private void ReflowLines(int init_line = 0) {
    this.GetLogger().Log("reflow called on base handler");
    if ((bounds_ == Vector2.Zero) || (Content.Length <= 0)) {
      this.lines = [];
      this.line_starts = [];
      return;
    }

    Vector2 string_dims;

    IList<string> lines = [];
    IList<int> line_starts = [];

    int line_skip = Math.Clamp(init_line, 0, Math.Max(this.lines.Count - 1, 0));

    for (int i = 0; i < line_skip; i++) {
      lines.Add(this.lines[i]);
      line_starts.Add(this.line_starts[i]);
    }

    int line_start = this.line_starts.Count > 0 ? this.line_starts[line_skip] : 0;
    int current_break = (line_start > 0) ? line_start - 1 : -1;

    do {
      // break at either the next newline char, or the next space char
      bool has_newline = false;

      int next_break = Content.IndexOf(' ', current_break + 1);
      int next_line = Content.IndexOf(Environment.NewLine, current_break + 1);

      if (((next_line < next_break) || next_break < 0) && (next_line >= 0)) {
        next_break = next_line;
        has_newline = true;
      }

      // thinking:
      // - separate this part into something else
      // - clean this up
      // - later : 3

      int line_end = next_break < 0 ? Content.Length : next_break;

      // handle case where we get -1?
    
      string sub = Content[line_start..line_end];
      string_dims = helper.GetStringSizePx(sub, Font, 1.0f);

      bool in_bounds = string_dims.X < bounds_.X;
      
      // exception case: last word in a line doesn't break, when it could
      // leave it for now
      bool should_break = (has_newline || !in_bounds);
      // (next break >= 0) is the bug
      // when next break is -1 and should_break is otherwise true

      if (should_break) {
        if (has_newline && in_bounds && (next_break >= 0)) {
          // break for newline - next word up to the new line is still in bounds
          current_break = next_break + (Environment.NewLine.Length - 1);
        }

        if (current_break < line_start) {
          // ie: single-word case
          current_break = next_break;
          // bug when it's the last word
        }

        lines.Add(Content[line_start..current_break]);
        line_starts.Add(line_start);
        // wind back and start a new line, re-parsing the current break
        line_start = current_break + 1;
      } else {
        // still good - advance another word
        current_break = next_break;
      }
    } while (current_break >= 0 && current_break < Content.Length);

    if (current_break != line_start) {
      lines.Add(Content[line_start..]);
      line_starts.Add(line_start);
    }

    this.lines = lines;
    this.line_starts = line_starts;
  }
}