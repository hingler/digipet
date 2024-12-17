using System;
using System.Collections.Generic;
using digipet.transition.animator;

namespace digipet.transition.state;

public class TransitionState : ITransition {
  private readonly IList<IAnimator> animators;
  private readonly double duration;
  private readonly double pause;
  private bool hold;

  private double dt = 0.0;

  public bool BlockInput { get; set; }

  public TransitionState(
    IList<IAnimator> animators,
    double duration,
    double pause,
    bool hold,
    bool block_input = false
  ) {
    this.animators = animators;
    this.duration = duration;
    this.pause = pause;
    this.hold = hold;

    BlockInput = block_input;
  }

  // returns true if anim is completed
  public void Tick(double delta) {
    dt += delta;
    double mix = Math.Clamp(dt / Math.Max(duration, 0.00001), 0.0, 1.0);
    foreach (IAnimator a in animators) {
      a.Mix((float)mix);
    }
  }

  public void Advance() {
    if (dt < duration + pause) {
      // if animation still running, then skip to end
      dt = duration + pause + 0.0001;
    } else {
      // otherwise, mark the hold as acknowledged
      // (if hold was already false, then we'll just jump to the next state)x
      hold = false;
    }
  }

  public bool Complete() {
    return (dt > (duration + pause)) && !hold;
  }
}