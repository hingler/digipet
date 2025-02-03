using System.Numerics;
using digipet.util;
using digipet.world;

namespace digipet.src.sim.toy.handlers;

public class TargetController {
  private float target_x;
  private float target_y;
  private double hold_delta;

  public float DistanceThreshold = 0.05f;
  public float TargetHeight = 0.3f;
  public float HoldDurationMedian = 2.0f;

  private readonly Random random = new();

  public Vector2 Target => new(target_x, target_y);
  
  public TargetController(float init_x) {
    target_x = init_x;
    target_y = TargetHeight;
    hold_delta = 0.0f;
  }

  public void Tick(double delta, IPositionable drone) {
    Vector2 dist = Target - drone.Position;

    if (dist.Length() > DistanceThreshold) {
      hold_delta = 0.0;
    } else {
      hold_delta += delta;
    }

    if (hold_delta > HoldDurationMedian) {
      // reshuffle target

      // prefer edges
      float random_sample = random.NextSingle() * 2.0f - 1.0f;
      random_sample = MathF.Sign(random_sample) * MathF.Pow(random_sample, 0.25f);

      // zig zag back and forth
      if (Math.Sign(target_x) == Math.Sign(random_sample)) {
        random_sample = -random_sample;
      }

      target_x = random_sample * 0.4f;
      target_y = random.NextSingle() * 0.2f + TargetHeight;
      hold_delta = 0.0;
    }
  }
}