using System.Collections.Generic;
using digipet.transition.helper;

namespace digipet.transition;

public class TransitionBuilder {

  private readonly IList<ITransition> states = [];

  public void Then(ITransition t) {
    states.Add(t);
  }

  public void AddState(ITransition t) => Then(t);
  public void ThenPause(double duration) => Then(new PauseTransition(duration));
  public void ThenHold() => Then(new HoldTransition());
  public void ThenCall(TransitionCallback callback) => Then(new CallbackTransition(callback));

  public ITransition Build() {
    return new TransitionSequence(states);
  }
}