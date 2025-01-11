using System.Numerics;
using digipet.rpg.context;

namespace digipet.rpg.util;

public static class RPGUtil {

  public static float GetDistance(ICharState ca, ICharState cb) {
    Vector2 bounds = TestBounds(ca, cb);
    return Math.Max(bounds.X - bounds.Y, 0.0f);
  }
  public static bool TestIntersection(ICharState ca, ICharState cb) {
    Vector2 bounds = TestBounds(ca, cb);

    return bounds.Y >= bounds.X;
  }
  public static Vector2 TestBounds(ICharState va, ICharState vb) => TestBounds(GetBounds(va), GetBounds(vb));
  public static Vector2 TestBounds(Vector2 va, Vector2 vb) {
    Vector2 b_intersect = new(
      Math.Max(va.X, vb.X),
      Math.Min(va.Y, vb.Y)
    );

    return b_intersect;
  } 

  public static Vector2 GetBounds(ICharState cs) {
    double width_a = cs.Position.X - (cs.Stats.Width / 2);
    double width_b = cs.Position.X + (cs.Stats.Width / 2);

    double width_min = Math.Min(width_a, width_b);
    double width_max = Math.Max(width_a, width_b);

    return new((float)width_min, (float)width_max);
  }
}