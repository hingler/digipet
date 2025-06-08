namespace digipet.sim.energy.impl;

public class SimpleLightModel : ILightModel {
  private readonly List<Action> callbacks = [];

  private bool lit_ = true;
  public bool Lit { 
    get => lit_;
    set {
      lit_ = value;
      foreach (Action cb in callbacks) {
        cb();
      }
    }
  }

  public bool IsLit() => Lit;
  public void Toggle(bool state) => Lit = state;
  public int GetTimeSpentLit(int lookback_sec) => Lit ? lookback_sec : 0;

  public void RegisterToggleListener(Action callback) => callbacks.Add(callback);
}