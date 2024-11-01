using digipet.transition;

// holds in this state until advance is called
public class HoldTransition : ITransition {
  private bool advanced;

  public HoldTransition() {
    advanced = false;
  }

  public void Tick(double delta) { /* no op */ }
  public void Advance() { advanced = true; }
  public bool Complete() => advanced;
}