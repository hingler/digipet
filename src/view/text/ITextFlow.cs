using System.Numerics;
using digipet.canvas.font;

namespace digipet.view.text;

public interface ITextFlow {
  public string Content { get; set; }
  public Vector2 Bounds { get; set; }
  public FontType Font { get; set; }

  public int LineSpacing { get; set; }

  // returns `Content` line by line
  public IReadOnlyList<string> GetLines();

  public float GetLineHeight();

  public IReadOnlyList<int> GetLineCharOffsets();

  // returns the baseline of the line, in px, based on an arbitrary "line_offset" specified by the user.
  public float GetTextBaseline(int line_num, float line_offset);

  // returns `Content` as a single string
  public string GetLinesAsString();

  public int GetMaxLinesVisible();
}