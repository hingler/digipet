using System;

namespace digipet.util;

public class Lerper {
  private double offset_;
  public double Cursor { get => offset_; }
  public double Target;
  // (encode offset from menu top)
  private readonly double SmoothingFactor;

  public Lerper(
    double smoothing_factor
  ) {
    SmoothingFactor = smoothing_factor;
    offset_ = 0.0f;
  }

  public static double Lerp(double t, double a, double b) {
    return a * (1.0 - t) + b * t;
  }

  public void Tick(double delta) {
    double dt = Math.Exp(delta * -Math.Max(SmoothingFactor, 0.000001));
    offset_ = Lerp(dt, Target, offset_);
  }
}