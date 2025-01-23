using System.Numerics;

namespace digipet.view.rpg;

public interface IRPGDisplay {
  public Vector2 WorldOrigin { get; set; }
  public float WorldScale { get; set; }
  public Vector2 SizePx { get; set; }
}

public static class RPGExtension {
  public static Vector2 Project(this IRPGDisplay display, Vector2 point_world) {
    Vector2 flip_pos = point_world - display.WorldOrigin;
    flip_pos.Y *= -1;
    Vector2 dist = flip_pos / display.WorldScale;
    return dist + (display.SizePx / 2);
  }
}