using System.Text;
using digipet.util;

namespace digipet.debug;

public class DrawCallCounter {
  private int line_calls;
  private int rect_calls;
  private int tex_calls;
  private int text_calls;

  public DrawCallCounter() {
    Reset();
  }

  public void Reset() {
    line_calls = 0;
    rect_calls = 0;
    tex_calls = 0;
    text_calls = 0;
  }

  public void Line() => ++line_calls;

  public void Rect() => ++rect_calls;
  public void Tex() => ++tex_calls;
  public void Text() => ++text_calls;

  public override string ToString() {
    string res = line_calls + " // " + rect_calls + " // " + tex_calls + " // " + text_calls;
    return res;
  }
}