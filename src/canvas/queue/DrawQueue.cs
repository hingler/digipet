using System.Collections.Generic;

namespace digipet.canvas.queue;

public class DrawQueue {
  private readonly PriorityQueue<IDrawItem, DrawRequest> queue = new(new DrawReqComparer());
  public void Enqueue(IDrawItem item, DrawRequest request) {
    queue.Enqueue(item, request);
  }

  public dynamic Dequeue() {
    return queue.Dequeue();
  }

  public int Count { get => queue.Count; }

  public void Clear() {
    queue.Clear();
  }
}