using System.Collections.Generic;
using digipet.transition;

namespace digipet.component;

public class TransitionQueue {
  private readonly Queue<ITransition> transitions = new();

  public void Enqueue(ITransition transition) {
    transitions.Enqueue(transition);
  }

  public void Advance() {
    if (transitions.TryPeek(out ITransition transition)) {
      transition.Advance();
    }
  }

  public bool Complete() {
    return (
      !transitions.TryPeek(out ITransition transition) 
      || transition.Complete() && transitions.Count == 1
    );
  }

  public bool BlockInput => transitions.TryPeek(out ITransition t) && t.BlockInput;

  public void Clear() {
    transitions.Clear();
  }

  public void Tick(double delta) {
    while (transitions.TryPeek(out ITransition transition) && transition.Complete()) {
      transitions.Dequeue();
    }

    if (transitions.Count > 0) {
      transitions.Peek().Tick(delta);
    }
  }
}