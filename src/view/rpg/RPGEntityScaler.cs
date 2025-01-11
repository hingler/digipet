
using digipet.rpg;
using digipet.util;

namespace digipet.view.rpg;

public class RPGEntityScaler {
  private readonly CombatManager manager;
  private readonly IRPGDisplay entity_view;
  public float Padding = 1.5f;
  public float MinScale = 0.05f;
  public float MaxScale = 0.25f;
  
  private readonly Lerper scale_lerp;

  public RPGEntityScaler(
    CombatManager manager,
    IRPGDisplay view
  ) {
    this.manager = manager;
    entity_view = view;
    scale_lerp = new(2.0);

    scale_lerp.Target = 0.1f;
    scale_lerp.Reset();
  }

  public void UpdateScale(double delta) {
    float min = 2000f;
    float max = -2000f;

    foreach (ICombatEntity entity in manager.GetEntities()) {
      min = Math.Min(entity.Position.X, min);
      max = Math.Max(entity.Position.X, max);
    }

    float view_width = (max - min) + 2.0f * Padding;

    float view_center = min + (max - min) / 2;

    float target_scale = view_width / entity_view.SizePx.X;

    entity_view.WorldOrigin = new(view_center, 0.0f);

    scale_lerp.Target = Math.Clamp(target_scale, MinScale, MaxScale);
    scale_lerp.Tick(delta);
    entity_view.WorldScale = (float)scale_lerp.Cursor;
  }
}