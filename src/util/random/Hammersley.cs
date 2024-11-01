using System.Numerics;

namespace digipet.util.random;

public static class Hammersley {
  private static long BitReverse(long x) {
    x = ((x & 0x0000FFFF) << 16) | ((x & 0xFFFF0000) >> 16);
    x = ((x & 0x00FF00FF) << 8)  | ((x & 0xFF00FF00) >> 8);
    x = ((x & 0x0F0F0F0F) << 4)  | ((x & 0xF0F0F0F0) >> 4);
    x = ((x & 0x33333333) << 2)  | ((x & 0xCCCCCCCC) >> 2);
    x = ((x & 0x55555555) << 1)  | ((x & 0xAAAAAAAA) >> 1);

    return x;
  }

  public static Vector2 GetHammersley(long x, long n) {
    long y = BitReverse(x);
    double x_frac = (double)x / n;
    double y_frac = (double)y / 0x100000000;

    return new((float)x_frac, (float)y_frac);
  }
}