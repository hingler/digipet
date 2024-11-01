using System.Collections.Generic;
using System.Numerics;
using System.Reflection;
using digipet.transition.animator;
using digipet.transition.helper;
using digipet.util;

namespace digipet.transition.state;


public class TransitionStateBuilder {

  private readonly IList<IAnimator> animators = [];

  private double duration = 0.0;

  private double pause = 0.0;
  private bool wait = false;
  private ILogger logger = LoggerSingleton.GetLogger();

  // split into "animateproperty" and "animatefield"
  public TransitionStateBuilder Animate<T>(
    object o, 
    string propertyName, 
    T from, 
    T to
  ) where T : IFloatingPoint<T> {
    return Animate(o, propertyName, from, to, EasingFunctions.Linear);
  }

  public TransitionStateBuilder Animate<T>(
    object o,
    string propertyName,
    T from,
    T to,
    EasingFunction ease
  ) where T : IFloatingPoint<T> {
    IMemberWrap<T> target;
    target = new FieldWrap<T>(o.GetType().GetField(propertyName), o);
    if (!target.Valid()) {
      target = new PropertyWrap<T>(o.GetType().GetProperty(propertyName), o);
    }

    if (!target.Valid()) {
      logger.Error("could not infer type of field ", propertyName, " on type ", o.GetType());
      target = new FallbackFieldWrap<T>();
    }

    LerpAnimator<T> anim = new(
      target,
      from,
      to,
      ease
    );

    animators.Add(anim);
    return this;
  }

  // duration of this animation
  public TransitionStateBuilder WithDuration(double duration) {
    this.duration = duration;
    return this;
  }

  // wait for advance before progressing
  public TransitionStateBuilder AndWait() {
    wait = true;
    return this;
  }

  public TransitionState Build() {
    return new(
      animators, duration, pause, wait
    );
  }
}