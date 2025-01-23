using digipet.framework;
using digipet.world;

namespace digipet.sim.toy;

public abstract class BaseToyHandler {
  private bool active_ = false;
  public bool Active { 
    get => active_;
    set {
      if (value != active_) {
        active_ = value;

        if (value) {
          Activate();
        } else {
          Deactivate();
        }
      }
    }
  }

  public abstract void Tick(double delta);

  protected abstract void Activate();
  protected abstract void Deactivate();

  public abstract void Destroy();
}