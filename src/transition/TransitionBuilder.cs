using System.Collections.Generic;
using digipet.transition.helper;

namespace digipet.transition;

public class TransitionBuilder {

  private readonly IList<ITransition> states = [];

  public void Then(ITransition t) {
    states.Add(t);
  }

  public void AddState(ITransition t) => Then(t);

  public void Pause(double duration) => ThenPause(duration);
  public void ThenPause(double duration) => Then(new PauseTransition(duration));

  public void Hold() => ThenHold();
  public void ThenHold() => Then(new HoldTransition());

  public void Call(TransitionCallback callback) => ThenCall(callback);
  public void ThenCall(TransitionCallback callback) => Then(new CallbackTransition(callback));

  public ITransition Build() {
    return new TransitionSequence(states);
  }
}