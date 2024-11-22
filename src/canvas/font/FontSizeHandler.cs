namespace digipet.canvas.font;

public class FontSizeHandler {
  private static readonly Dictionary<FontType, int> FONT_SIZE = new() {
    {FontType.TINY, 11},
    {FontType.SMALL, 18},
    {FontType.MID, 27},
    {FontType.LARGE, 34},
    {FontType.EXTRA_LARGE, 41}
  };

  public static int GetFontSize(FontType t) {
    return FONT_SIZE.GetValueOrDefault(t, FONT_SIZE[FontType.TINY]);
  }

  public static float GetFontAscent(FontType t) {
    float px_size = (float)t;
    float space = GetFontSize(t) - px_size;
    return px_size + (space / 2.0f);
  }

  public static float GetFontDescent(FontType t) {
    return (GetFontSize(t) - (float)t) / 2.0f;
  }
}