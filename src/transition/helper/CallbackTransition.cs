namespace digipet.transition.helper;
public delegate void TransitionCallback();

public class CallbackTransition : ITransition {

  private readonly TransitionCallback c;
  private bool called;

  public CallbackTransition(TransitionCallback c) {
    this.c = c;
    called = false;
  }

  public void Tick(double delta) {
    if (!called) {
      c();
      called = true;
    }
  }

  public void Advance() { /* no op */ }
  public bool Complete() => called;

  public bool BlockInput { get; set; } = false;
}