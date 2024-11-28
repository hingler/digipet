using System.Numerics;

namespace digipet.util;

public struct DigiColor {
  public float R, G, B, A;
  public DigiColor(float r, float g, float b, float a) {
    R = r;
    G = g;
    B = b;
    A = a;
  }

  public DigiColor(float shade) : this(shade, shade, shade, 1.0f) {}

  public readonly DigiColor WithOpacity(float opac) {
    return new DigiColor(R, G, B, A * opac);
  }

  public static implicit operator Vector4(DigiColor c) {
    return new Vector4(c.R, c.G, c.B, c.A);
  }

  public static implicit operator DigiColor(Vector4 c) {
    return new DigiColor(c.X, c.Y, c.Z, c.W);
  }

  public static readonly DigiColor WHITE = new(1.0f, 1.0f, 1.0f, 1.0f);
  public static readonly DigiColor BLACK = new(0.0f, 0.0f, 0.0f, 1.0f);
}