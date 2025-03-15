using System.Numerics;
using digipet.component;

namespace digipet.view.rpg;

public interface IRPGDisplay {
  public Vector2 WorldOrigin { get; set; }
  public float WorldScale { get; set; }
  public Vector2 SizePx { get; set; }

  public ViewComponent GetRootView();
}

public interface IPseudoDepthDisplay : IRPGDisplay {
  public float ZDist { get; }
  public float SpriteScale { get; }
}

public static class RPGExtension {
  // project into target's pixel-space
  public static Vector2 ProjectAbsolute(this IRPGDisplay display, Vector2 point_world) {
    Vector2 flip_pos = point_world - display.WorldOrigin;
    flip_pos.Y *= -1;
    Vector2 dist = flip_pos / display.WorldScale;
    return dist + (display.SizePx / 2);
  }

  // project into target's relative offset space
  public static Vector2 ProjectRelative(this IRPGDisplay display, Vector2 point_world) {
    return display.ProjectAbsolute(point_world) / display.SizePx;
  }
}