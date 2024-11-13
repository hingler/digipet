using System;

namespace digipet.util;

public static class Gaussian {
  public static double GetGaussian(Random r) {
    double u_1 = r.NextDouble();
    double u_2 = r.NextDouble();

    return Math.Sqrt(-2.0 * Math.Log(u_1)) * Math.Cos(2.0 * Math.PI * u_2);
  }
}