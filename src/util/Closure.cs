using System;

namespace digipet.util;

#nullable enable

public static class Closure {
  public static T Let<T> (this T target, Action<T> a) {
    a(target);
    return target;
  }

  public static void Let(Action a) {
    a();
  }
}