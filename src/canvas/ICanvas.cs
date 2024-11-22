using System.Numerics;
using digipet.canvas.font;
using digipet.image;
using digipet.view;

namespace digipet;

public enum HorizontalAlign {
  LEFT,
  CENTER,
  RIGHT
}

public interface ICanvas {
  // draw a colored rect onto the screen
  void Rect(Vector2 start, Vector2 end, int border_radius, Vector4 col, int z_index = 0);

  // draw text onto the screen
  void Text(
    Vector2 origin, 
    string text, 
    float scale, 
    FontType typeface,
    HorizontalAlign alignment,
    Vector4 color,
    int z_index = 0
  );

  void Line(Vector2 start, Vector2 end, float thickness, Vector4 col, int z_index = 0);

  void Tex(
    ISprite image, 
    Vector2 start, 
    Vector2 end, 
    bool tile
  ) => Tex(image, start, end, tile, Vector4.One);
  void Tex(ISprite image, Vector2 start, Vector2 end, bool tile, Vector4 modulate, int z_index = 0);

  // tba: drawing images?

  public Vector2 GetStringSize(Text text) => GetStringSize(text.Content, text.Font, text.Scale);
  public Vector2 GetStringSize(string text, FontType typeface, float scale) => GetStringSize(text, typeface, scale, -1, -1);
  public Vector2 GetStringSize(string text, FontType typeface, float scale, float width_px, int max_lines);

  // called at the end of a "draw frame"
  void Flush();

  Vector2 GetSizePx();

  Vector2 GetPixelDims() {
    Vector2 size = GetSizePx();
    return new(1.0f / size.X, 1.0f / size.Y);
  }

  Vector2 PxToRelative(Vector2 v) {
    return PxToRelative(v.X, v.Y);
  }

  Vector2 PxToRelative(float x, float y) {
    Vector2 dims = GetPixelDims();
    return new Vector2(x * dims.X, y * dims.Y);
  }
}