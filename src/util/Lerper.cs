using System;
using System.Numerics;

namespace digipet.util;

public interface ILerper<T> {
  public T Cursor { get; }
  public T Target { get; set; }

  public void Reset();
  public void Tick(double delta);
}

public class Vec2Lerper(double smoothing_factor) : ILerper<Vector2> {
  private readonly Lerper lerp_x = new(smoothing_factor);
  private readonly Lerper lerp_y = new(smoothing_factor);

  public Vector2 Target {
    get => new((float)lerp_x.Target, (float)lerp_y.Target);
    set {
      lerp_x.Target = value.X;
      lerp_y.Target = value.Y;
    }
  }

  public Vector2 Cursor => new((float)lerp_x.Cursor, (float)lerp_y.Cursor);

  public void Reset() {
    lerp_x.Reset();
    lerp_y.Reset();
  }

  public void Tick(double delta) {
    lerp_x.Tick(delta);
    lerp_y.Tick(delta);
  }
}

public class Lerper : ILerper<double> {
  private double offset_;
  public double Cursor { 
    get => offset_; 
  }
  public double Target { get; set; }
  // (encode offset from menu top)
  private readonly double SmoothingFactor;

  public Lerper(
    double smoothing_factor
  ) {
    SmoothingFactor = smoothing_factor;
    offset_ = 0.0f;
  }

  public void Reset() {
    offset_ = Target;
  }

  public static double Lerp(double t, double a, double b) {
    return a * (1.0 - t) + b * t;
  }

  public void Tick(double delta) {
    offset_ = LerpValue(delta, offset_, SmoothingFactor, Target);
  }

  public static double LerpValue(double delta, double input, double smoothing, double target) {
    double dt = Math.Exp(delta * -Math.Max(smoothing, 0.000001));
    return Lerp(dt, target, input);
  }
}