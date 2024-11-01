using System;
using System.Collections.Generic;
using System.Numerics;
using digipet.component;
using digipet.transition.animator;
using digipet.util.random;

namespace digipet.view.sky;

public class StarrySky : ViewComponent {

  private readonly IList<Vector4> stars = [];

  private static readonly Vector4 ColDark = new(0.5f, 0.5f, 0.5f, 1.0f);
  private static readonly Vector4 ColBright = new(1.0f, 1.0f, 1.0f, 1.0f);

  private double dt;

  public float fade_start = 0.6f;
  public float fade_end = 0.9f;

  public float StarOffset = 0.0f;

  public StarrySky(
    int density
  ) {
    GenerateStars(density);
    dt = 0.0;
  }

  private void GenerateStars(int density) {
    int star_count = 1 << density;
    Random r = new();
    for (int i = 0; i < star_count; i++) {
      Vector2 star_pos = Hammersley.GetHammersley(i, star_count);
      float intensity = MathF.Pow(r.NextSingle(), 3.0f) * 5.0f;
      float phase = r.NextSingle() * 2.0f * MathF.PI;

      stars.Add(new Vector4(star_pos.X, star_pos.Y, intensity, phase));
    }
  }

  public override void Tick(double delta) {
    base.Tick(delta);
    dt += delta;
  }

  public override void Draw(ICanvas canvas) {
    base.Draw(canvas);
    canvas.Rect(Vector2.Zero, Vector2.One, 0, Vector4.UnitW);

    Vector2 frag_coord = canvas.GetPixelDims();

    // stars

    for (int i = 0; i < stars.Count; i++) {
      Vector4 star = stars[i];
      int intensity = (int)star.Z;

      float alph = 1.0f - (float)EasingFunctions.SmoothStep(fade_start, fade_end, star.Y);
      float twinkle = (float)Math.Sin(2.1 * dt + star.W) * 0.3f + 0.7f;
      alph *= twinkle;

      Vector4 cmod = new(alph, alph, alph, 1.0f);
      Vector2 star_pos = new(star.X, star.Y);

      star_pos.Y += StarOffset;

      switch (intensity) {
        case 0:
          canvas.Rect(star_pos, star_pos + frag_coord, 0, ColDark * cmod);
          break;
        case 1:
          canvas.Rect(star_pos, star_pos + frag_coord, 0, ColBright * cmod);
          break;
        case 2:
          canvas.Rect(star_pos, star_pos + frag_coord * 2.0f, 0, ColDark * cmod);
          canvas.Rect(star_pos + frag_coord, star_pos + 2.0f * frag_coord, 0, ColBright * cmod);
          break;
        case 3:
          canvas.Rect(star_pos, star_pos + frag_coord * 2.0f, 0, ColBright * cmod);
          canvas.Rect(star_pos, star_pos + frag_coord, 0, ColDark * cmod);
          break;
        case 4:
        default:
          canvas.Rect(star_pos, star_pos + frag_coord * 2.0f, 0, ColBright * cmod);
          break;
      }
    }
  }
}