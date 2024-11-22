using System.Numerics;
using digipet.canvas.font;
using digipet.component;
using digipet.framework;
using digipet.util;

namespace digipet.view.text;

public class TextFlowHandler : ITextFlow {
  private string content_ = "";
  public string Content {
    get => content_;
    set {
      content_ = value;
      ReflowLines();
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

  private readonly IFontHelper helper;

  private IList<string> lines;

  public TextFlowHandler(IEngine engine) {
    helper = engine.GetFontHelper();
    lines = [];
  }

  public IReadOnlyList<string> GetLines() {
    if (lines.Count <= 0) {
      ReflowLines();
    }

    return lines.AsReadOnly();
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
    return FontSizeHandler.GetFontAscent(Font) + FontSizeHandler.GetFontDescent(Font);
  }

  private int GetMaxLinesVisible_Absolute() {
    return (int)Math.Floor(Bounds.Y / GetLineHeight());
  }

  public int GetMaxLinesVisible() {
    int max_lines = GetMaxLinesVisible_Absolute();
    return Math.Min(max_lines, GetLines().Count);
  }

  private void ReflowLines() {
    if ((bounds_ == Vector2.Zero) || (Content.Length <= 0)) {
      return;
    }

    int line_start = 0;
    int current_break = 0;
    Vector2 string_dims;

    IList<string> lines = [];

    while (current_break != -1) {
      int next_break = Content.IndexOf(' ', current_break + 1);
      int line_end = next_break < 0 ? Content.Length - 1 : next_break;
      // handle case where we get -1?
    
      string sub = Content[line_start..line_end];
      string_dims = helper.GetStringSizePx(sub, FontType.TINY, 1.0f);

      if ((string_dims.X >= bounds_.X) && (line_start != current_break)) {
        // we've shot past it!
        lines.Add(Content[line_start..current_break].Trim());
        // wind back and start a new line
        line_start = current_break;
      } else {
        // still good - advance another word
        current_break = next_break;
      }
    }

    if (current_break != line_start) {
      lines.Add(Content[line_start..].Trim());
    }

    this.lines = lines;
  }
}