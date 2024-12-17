using System.Collections.Generic;

namespace digipet.transition;

public class TransitionSequence : ITransition {
  private readonly Queue<ITransition> states;

  public TransitionSequence(
    IList<ITransition> states
  ) {
    this.states = new();
    foreach (ITransition t in states) {
      this.states.Enqueue(t);
    }
  }

  public void Tick(double delta) {
    if (states.Count > 0) {
      ITransition tran = states.Peek();
      tran.Tick(delta);

      while (tran.Complete() && states.Count > 0) {
        states.Dequeue();
        if (states.Count > 0) {
          tran = states.Peek();
        }
      }
    }
  }

  public void Advance() {
    states.Peek().Advance();
  }

  public bool Complete() {
    return states.Count == 0;
  }

  public bool BlockInput => states.Peek()?.BlockInput ?? false;
}