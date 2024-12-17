using digipet.framework;
using digipet.transition.animator;
using digipet.util;
using digipet.view.text;

namespace digipet.transition.text;

#nullable enable

public class DialogueTransition : ITransition {
  private readonly TextFlowAnimator animator;
  private readonly DialogueSubstate dialogue_state;
  private DialogueSubScroll? dialogue_scroll;

  public float LineOffset {
    get {
      if (dialogue_scroll != null) {
        return dialogue_scroll.Offset;
      } else {
        return animator.LineOffset;
      }
    }
  }

  public double AnimationSpeed {
    get => animator.AnimationSpeed;
    set => animator.AnimationSpeed = value;
  }

  public string Content {
    get => animator.Content;
    set {
      animator.Content = value;
      Reset();
    }
  }

  public DialogueTransition(
    IEngine engine
  ) {
    animator = new(engine) {
      Content = ""
    };
    
    dialogue_state = new(animator);
    dialogue_scroll = null;

    animator.Reset();
  }

  public void Reset() {
    animator.Reset();
    dialogue_scroll = null;
  }

  public ITextFlow GetAnimator() => animator;

  public void Tick(double delta) {
    dialogue_scroll?.Tick(delta);
    if (dialogue_scroll?.Complete() ?? false) {
      animator.LineOffset = (int)dialogue_scroll.Offset;
      dialogue_scroll = null;
    } else if (dialogue_scroll == null) {
      dialogue_state.Tick(delta);
      if (dialogue_state.Complete() && !animator.Complete()) {
        CreateNewScroller();
      }
    }
  }

  private void CreateNewScroller() {
    int lines_visible =  animator.GetMaxLinesVisible();
    int line_count = animator.GetTotalLines();
    int current_offset = animator.LineOffset;

    int first_line_not_visible = current_offset + lines_visible;
    int offset_advance = Math.Min(Math.Max((int)Math.Floor(lines_visible / 2.0f), 1), line_count - first_line_not_visible);

    dialogue_scroll = new(current_offset, current_offset + offset_advance, 0.1, EasingFunctions.EaseInOutQuad);
  }

  public void Advance() {
    if (dialogue_scroll == null) {
      dialogue_state.Advance();
    }
  }

  public bool Complete() => animator.Complete();

  public bool BlockInput => false;
}