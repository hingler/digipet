using digipet.transition.animator;
using digipet.transition.helper;

namespace digipet.transition.text;

public class ScrollDelegateWrap : IMemberWrap<float>  {
  public float Value;
  public void SetValue(float value) {
    Value = value;
  }

  public bool Valid() => true;
}

public class DialogueSubScroll : ITransition {
  private readonly LerpAnimator<float> lerper;
  private readonly double duration;
  private readonly ScrollDelegateWrap wrap;
  private double dt = 0;

  public float Offset => wrap.Value;

  public DialogueSubScroll(
    int start, int end, double duration, EasingFunction ease
  ) {
    wrap = new() {
      Value = start
    };

    this.duration = duration;

    lerper = new(
      wrap, start, end, ease
    );
  }

  public void Tick(double delta) {
    dt += delta;
    lerper.Mix((float)(dt / duration));
  }

  public void Advance() {
    // do nothing
  }

  public bool Complete() {
    return dt > duration;
  }
}