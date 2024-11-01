using System.Collections.Generic;

namespace digipet.canvas.queue;

public struct DrawRequest {
  public DrawType Type;
  public int ZIndex;
  public int Count;
}

public class DrawReqComparer : IComparer<DrawRequest> {
  public int Compare(DrawRequest x, DrawRequest y) {
    if (x.ZIndex != y.ZIndex) {
      return x.ZIndex.CompareTo(y.ZIndex);
    } else if (x.Count != y.Count) {
      return x.Count.CompareTo(y.Count);
    } else {
      return 0;
    }
  }
}