using System.Numerics;
using digipet.canvas.font;
using digipet.image;
using digipet.util;
using digipet.view;

namespace digipet.canvas;

public class OffsetCanvas : ICanvas {
  private readonly Vector2 offset;

  // size defined here from 0.0 - 1.0
  private readonly Vector2 size;
  private readonly float scale;
  private readonly ICanvas canvas;

  private readonly Vector4 mod_alpha;
  private readonly int z_index;

  public OffsetCanvas(
    ICanvas base_canvas, 
    Vector2 offset, 
    Vector2 size, 
    float scale,
    float alpha,
    int z_index
  ) {
    this.offset = offset;
    this.scale = scale;
    this.size = size;
    this.z_index = z_index;
    canvas = base_canvas;

    mod_alpha = new(1.0f, 1.0f, 1.0f, alpha);
  }

  private Vector2 Project(
    Vector2 input
  ) {
    // (conv from this offset canvas's space, to its parent's)
    return (scale * (size * input)) + offset;
  }

  public void Rect(
    Vector2 start,
    Vector2 end,
    int border_radius,
    Vector4 col, 
    int z_index
  ) {

    if (mod_alpha.W > 0.001f) {
      canvas.Rect(
        Project(start),
        Project(end),
        border_radius,
        col * mod_alpha,
        z_index + this.z_index
      );
    }
  }

  public void Text(
    Vector2 origin,
    string text,
    float scale,
    FontType typeface,
    HorizontalAlign alignment,
    Vector4 color, 
    int z_index
  ) {
    if (mod_alpha.W > 0.001f) {
      canvas.Text(
        Project(origin),
        text, 
        scale * this.scale,
        typeface,
        alignment,
        color * mod_alpha,
        z_index + this.z_index
      );
    }
  }

  public void Line(
    Vector2 start, Vector2 end, float thickness, Vector4 col, int z_index
  ) {
    if (mod_alpha.W > 0.001f) {
      canvas.Line(
        Project(start), 
        Project(end), 
        thickness, 
        col * mod_alpha,
        z_index + this.z_index
      );
    }
  }

  public void Tex(
    ISprite image, Vector2 start, Vector2 end, bool tile, Vector4 modulate, int z_index
  ) {
    if (mod_alpha.W > 0.001f) {
      canvas.Tex(
        image, 
        Project(start), 
        Project(end), 
        tile, 
        modulate * mod_alpha,
        z_index + this.z_index
      );
    }
  }

  public Vector2 GetStringSize(Text text) => GetStringSize(text.Content, text.Font, text.Scale);
  public Vector2 GetStringSize(string text, FontType typeface, float scale) => GetStringSize(text, typeface, scale, -1, -1);

  public Vector2 GetStringSize(
    string text,
    FontType typeface,
    float scale,
    float width_px,
    int max_lines
  ) {
    // string size wrt parent canvas
    Vector2 res = canvas.GetStringSize(text, typeface, scale, width_px, max_lines);
    return res / (size * this.scale);
  }

  public void Flush() {
    canvas.Flush();
  }

  public Vector2 GetSizePx() {
    return canvas.GetSizePx() * size;
  }
}