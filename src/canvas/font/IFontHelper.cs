using System.Numerics;
using digipet.view;

namespace digipet.canvas.font;

public interface IFontHelper {
  public Vector2 GetStringSizePx(Text text) => GetStringSizePx(text.Content, text.Font, text.Scale);
  public Vector2 GetStringSizePx(string text, FontType typeface, float scale) => GetStringSizePx(text, typeface, scale, -1, -1);
  public Vector2 GetStringSizePx(string text, FontType typeface, float scale, float width_px = -1, int max_lines = -1);
}