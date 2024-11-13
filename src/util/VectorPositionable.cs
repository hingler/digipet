using System.Numerics;

namespace digipet.util;

public class VectorPositionable(Vector2 pos) : IPositionable {
  public Vector2 Position => pos;
  public Vector2 Velocity => Vector2.Zero;
}