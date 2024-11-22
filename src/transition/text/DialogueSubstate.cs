using digipet.view.text;

namespace digipet.transition.text;

public class DialogueSubstate : ITransition {
  private readonly TextFlowAnimator animator;

  public DialogueSubstate(TextFlowAnimator animator) {
    this.animator = animator;
  }

  public void Tick(double delta) {
    animator.Update(delta);
  }

  public void Advance() {
    if (!animator.Waiting()) {
      animator.AdvanceToNextStop();
    }
  }

  public bool Complete() => animator.Waiting();
}

