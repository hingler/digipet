using System.Numerics;
using digipet.component;
using digipet.util;
using digipet.view.rpg;

namespace digipet.view.gpr;


public class RPGGrid : ViewComponent, IRPGDisplay {
  public Vector2 WorldOrigin { get; set; }
  public float WorldScale { get; set; }

  public RPGGrid() {}

  public override void Draw(ICanvas canvas) {
    base.Draw(canvas);
    
    Vector2 view_size = SizePx * WorldScale;

    // draw lines on 1's, light
    Vector2 bottom_corner = WorldOrigin - view_size / 2;

    Vector2 top_corner = WorldOrigin + view_size / 2;

    for (int x = (int)Math.Ceiling(bottom_corner.X); x < top_corner.X; x++) {
      float opac = (x % 5 == 0 ? 1 : 0.375f);

      float rel_dist = (x - bottom_corner.X) / view_size.X;
      canvas.Line(
        new Vector2(rel_dist, 0.0f), 
        new Vector2(rel_dist, 1.0f), 
        1.0f, 
        DigiColor.BLACK.WithOpacity(opac)
      );
    }
  }


}