using System.Numerics;

namespace digipet.util;

public interface IPositionable {
  Vector2 Position { get; }
  Vector2 Velocity { get; }
}