using System.Collections.Generic;

namespace digipet.util;

public class MaxComparer<T>(IComparer<T> comparer) : IComparer<T> {
  public MaxComparer() : this(Comparer<T>.Default) {}

  public int Compare(T x, T y) {
    return comparer.Compare(y, x);
  }
}