using System.Numerics;
using digipet.framework;

namespace digipet.view.rpg;

public class RPGDisplayDelegate {
  private readonly List<IRPGDisplay> displays = [];
  private Vector2 size_px_;
  private float world_scale_;
  private Vector2 world_origin_;

  public Vector2 SizePx {
    get => size_px_;
    set {
      size_px_ = value;
      foreach (IRPGDisplay display in displays) {
        display.SizePx = value;
      }
    }
  }

  public float WorldScale {
    get => world_scale_;
    set {
      world_scale_ = value;
      foreach (IRPGDisplay display in displays) {
        display.WorldScale = value;
      }
    }
  }

  public Vector2 WorldOrigin {
    get => world_origin_;
    set {
      world_origin_ = value;
      foreach (IRPGDisplay display in displays) {
        display.WorldOrigin = value;
      }
    }
  }

  public RPGDisplayDelegate(IEngine engine) {
    WorldOrigin = Vector2.Zero;
    WorldScale = 0.1f;

    // is this a reasonable default??
    size_px_ = engine.GetScreenRes();
  }

  public void AddDisplay(IRPGDisplay display) {
    displays.Add(display);

    display.SizePx = size_px_;
    display.WorldScale = world_scale_;
    display.WorldOrigin = world_origin_;
  }
}