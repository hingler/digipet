using System;

namespace digipet.util;

#nullable enable

public static class Closure {
  public static void Let<T> (this T target, Action<T> a) {
    a(target);
  }
}