namespace digipet.util.text;

public static class TruncateEllipses {
  public static string Truncate(this string s, int threshold) {
    return (s.Length > threshold ? s[..(threshold - 1)] + "..." : s);
  }
}