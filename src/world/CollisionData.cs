using System.Numerics;

namespace digipet.world;

public struct CollisionData {
  // point of collision
  public Vector2 Point;
  // collision normal
  public Vector2 Normal;

  // impulse applied after collision
  public Vector2 Impulse;

  // position change
  public Vector2 DeltaPos;

  public CollisionData() {
    Point = Vector2.Zero;
    Normal = Vector2.Zero;
    Impulse = Vector2.Zero;
    DeltaPos = Vector2.Zero;
  }
}