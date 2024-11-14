using System.Collections.Generic;

namespace digipet.util;

#nullable enable

public class MaxComparer<T>(IComparer<T> comparer) : IComparer<T> {
  public MaxComparer() : this(Comparer<T>.Default) {}

  public int Compare(T? x, T? y) {
    if (x == null && y == null) {
      return 0;
    } else if (x != null && y == null) {
      return 1;
    } else if (x == null && y != null) {
      return -1;
    }
    
    return comparer.Compare(y, x);
  }
}