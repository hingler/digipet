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

  public void Tick(double delta) {
    double dt = Math.Exp(delta * -Math.Max(SmoothingFactor, 0.000001));
    offset_ = Target * (1.0 - dt) + offset_ * dt;
  }
}