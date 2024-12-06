using System.Numerics;

namespace digipet.util;

public static class DigiMath {
  public static T PositiveModulo<T>(T val, T divisor) where T : INumber<T> {
    return ((val % divisor) + divisor) % divisor;
  }
}