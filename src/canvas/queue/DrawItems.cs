using System.Numerics;
using digipet.canvas.font;
using digipet.image;

namespace digipet.canvas.queue;

public interface IDrawItem {
}

public struct RectItem : IDrawItem {
  public Vector2 start, end;
  public Vector4 col;
  public int radius;
}

public struct TextItem : IDrawItem {
  public Vector2 origin;
  public string content;
  public float scale;
  public float width_px;
  public FontType typeface;
  public HorizontalAlign alignment;
  public Vector4 color;
}

public struct LineItem : IDrawItem {
  public Vector2 start, end;
  public float thickness;
  public Vector4 col;
}

public struct TexItem : IDrawItem {
  public ISprite image;
  public Vector2 start;
  public Vector2 end;
  public bool tile;
  public Vector4 modulate;
}