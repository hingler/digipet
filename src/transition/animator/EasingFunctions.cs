using System;

namespace digipet.transition.animator;

public delegate double EasingFunction(double factor);

public static class EasingFunctions {
  public static double Linear(double factor) => factor;
  public static double EaseInOut(double factor, double pow) {
    if (factor < 0.5) {
      return 4.0 * Math.Pow(factor, pow);
    } else {
      return 1.0 - Math.Pow(-2 * factor + 2, pow) / 2.0;
    }
  }

  public static double EaseInOutQuad(double factor) => EaseInOut(factor, 2.0);
  public static double EaseInOutCubic(double factor) => EaseInOut(factor, 3.0);
  public static double EaseInOutQuart(double factor) => EaseInOut(factor, 4.0);
  public static double EaseInOutQuint(double factor) => EaseInOut(factor, 5.0);

  public static double EaseOut(double factor, double pow) {
    return 1.0 - Math.Pow(1.0 - factor, pow);
  }

  public static double EaseOutQuad(double factor) => EaseOut(factor, 2.0);
  public static double EaseOutCubic(double factor) => EaseOut(factor, 3.0);
  public static double EaseOutQuart(double factor) => EaseOut(factor, 4.0);
  public static double EaseOutQuint(double factor) => EaseOut(factor, 5.0);

  public static double EaseIn(double factor, double pow) {
    return Math.Pow(factor, pow);
  }

  public static double EaseInQuad(double x) => EaseIn(x, 2);
  public static double EaseInCubic(double x) => EaseIn(x, 3);
  public static double EaseInQuart(double x) => EaseIn(x, 4);
  public static double EaseInQuint(double x) => EaseIn(x, 5);

  public static double EaseInBack(double fac, double pow) {
    double c1 = 1.70158 * (pow - 1);
    double c3 = c1 + 1;
    return (c3 * Math.Pow(fac, pow + 1)) - (c1 * Math.Pow(fac, pow));
  }

  public static double EaseInBackQuart(double x) => EaseInBack(x, 4);

  public static double SmoothStep(double a, double b, double t) {
    double ta = Math.Clamp((t - a) / (b - a), 0.0, 1.0);
    return ta * ta * ta * (ta * (ta * 6 - 15) + 10);
  }
}