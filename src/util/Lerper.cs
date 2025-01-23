using System;

namespace digipet.util;

public class Lerper {
  private double offset_;
  public double Cursor { 
    get => offset_; 
  }
  public double Target;
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