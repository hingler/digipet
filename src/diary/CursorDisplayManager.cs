using System.ComponentModel;
using System.Numerics;
using digipet.canvas.font;
using digipet.framework;
using digipet.util;
using digipet.view.text;

namespace digipet.diary;

public class CursorDisplayManager : ITextFlow {
  public readonly ITextEditor Editor;
  private readonly TextFlowHandler flow;

  private readonly IFontHelper helper;

  public int LineSpacing {
    get => flow.LineSpacing;
    set => flow.LineSpacing = value;
  }

  public FontType Font {
    get => flow.Font;
    set => flow.Font = value;
  }

  public Vector2 Bounds {
    get => flow.Bounds;
    set => flow.Bounds = value;
  }

  public string Content {
    get => Editor.AsString();
    set {
      Editor.Clear();
      Put(value);
    }
  }

  public float LineOffset = 0.0f;
  public int Length => Editor.Length();

  public ITextFlow Flow => flow;

  public CursorDisplayManager(IEngine engine, ITextEditor editor) {
    this.Editor = editor;
    helper = engine.GetFontHelper();
    flow = new TextFlowHandler(engine) {
      LineSpacing = 1
    };
  }

  public void Put(string s) {
    Editor.Put(s);
    flow.Content = Editor.AsString();
  }

  public void Delete() {
    Editor.Delete();
    flow.Content = Editor.AsString();
  }

  public void CursorUp() {
    if (GetLine() <= 0) {
      Editor.Cursor = 0;
    } else {
      int line = GetLine();
      int column = GetColumnForLine(line - 1);
      int offset = flow.GetLineCharOffsets()[line - 1];

      Editor.Cursor = offset + column;
    }
  }

  public void CursorDown() {
    if (GetLine() >= (flow.GetLines().Count - 1)) {
      Editor.Cursor = Editor.Length();
    } else {
      int line = GetLine();
      int column = GetColumnForLine(line + 1);
      int offset = flow.GetLineCharOffsets()[line + 1];

      Editor.Cursor = offset + column;
    }
  }

  public void CursorLeft() => --Editor.Cursor;
  public void CursorRight() => ++Editor.Cursor;
  
  public void SeekToNextWord() {
    WordResult res = Editor.GetNextWord();
    Editor.Cursor = res.EndIndex;
  }

  public void SeekToPreviousWord() {
    WordResult res = Editor.GetPreviousWord();
    Editor.Cursor = res.StartIndex;
  }

  public float GetCursorX() {
    int column = GetColumn();

    if (column == 0) {
      // left side of margin
      return 0;
    }

    int current_line = GetLine();
    string line_text = flow.GetLines()[current_line];

    string cursor_x = line_text[..column];
    return helper.GetStringSizePx(cursor_x, Font, 1.0f).X;
  }

  public float GetCursorY(float offset) {
    int line = GetLine();
    return flow.GetTextBaseline(line, offset);
  }

  public int GetLine() {
    int cur = Editor.Cursor;
    if (cur <= 0) {
      return 0;
    }

    IReadOnlyList<int> offsets = flow.GetLineCharOffsets();
    for (int i = 1; i <= offsets.Count; i++) {
      if ((i == offsets.Count) || (offsets[i] > cur)) {
        // if last char in this string is a newline, then jump to the next line
        string prev_line = flow.GetLines()[i - 1];
        if (prev_line.EndsWith(Environment.NewLine) && ((cur - offsets[i - 1]) >= prev_line.Length)) {
          return i;
        }

        return i - 1;
      }
    }
    return offsets.Count - 1;
  }

  public IReadOnlyList<string> GetLines() => flow.GetLines();
  public float GetLineHeight() => flow.GetLineHeight();
  public IReadOnlyList<int> GetLineCharOffsets() => flow.GetLineCharOffsets();
  public float GetTextBaseline(int line_num, float line_offset) => flow.GetTextBaseline(line_num, line_offset);
  public string GetLinesAsString() => flow.GetLinesAsString();
  public int GetMaxLinesVisible() => flow.GetMaxLinesVisible();

  public int GetColumn() {
    IReadOnlyList<int> offsets = flow.GetLineCharOffsets();
    int line = GetLine();
    if (Editor.Cursor == 0 || flow.GetLines().Count <= 0 || line >= offsets.Count) {
      return 0;
    }


    int offset = flow.GetLineCharOffsets()[line];

    string line_text = flow.GetLines()[line];

    return Math.Clamp(Editor.Cursor - offset, 0, line_text.Length);
  }

  private int GetColumnForLine(int desired_line) {
    IReadOnlyList<string> lines = flow.GetLines();
    if (desired_line < 0 || desired_line >= lines.Count) {
      return 0;
    }

    float width_x = GetCursorX() + 0.0001f;

    string target_line = lines[desired_line];
    float target_x = 0.0f;
    float prev_x = 0.0f;

    int target_col = 0;

    // when receiving string.empty: both == 0
    // lt width -> le width fixes it
    while ((target_x <= width_x) && (target_col < target_line.Length)) {
      target_col++;
      prev_x = target_x;
      target_x = helper.GetStringSizePx(target_line[..target_col], Font, 1.0f).X;
    }
    
    if (target_col >= target_line.Length) {
      return target_line.Length;
    }

    float dist_target = Math.Abs(target_x - width_x);
    float dist_prev = Math.Abs(prev_x - width_x);

    return (dist_target < dist_prev) ? target_col : target_col - 1;
  }
}