using System.Numerics;
using digipet;
using digipet.component;
using digipet.util;
using digipet.view.bg;
using digipet.view.rpg;

public class SolidColorBG : ViewComponent, IPseudoDepthDisplay {
  float z_dist = 1.0f;
  public float ZDist {
    get => z_dist;
    set {
      z_dist = MathF.Max(value, 0.00001f);
      // -1 -> 0
      ZIndex = -z_dist + 1;
    }
  }

  public bool LockX { get; set; }
  public bool LockY { get; set; }

  public Vector2 WorldOrigin { get; set; }
  public float WorldScale { get; set; }
  public float SpriteScale { get; set; }
  private readonly ColorRect rect;

  public ViewComponent GetRootView() => this;

  public DigiColor Color {
    get => rect.Color;
    set => rect.Color = value;
  }

  public SolidColorBG() {
    rect = new() {
      Size = Vector2.One,
      Offset = Vector2.Zero
    };

    AddView(rect);
  }
}