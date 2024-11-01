using System;
using System.Numerics;
using System.Reflection;
using digipet.transition.helper;
using digipet.util;

namespace digipet.transition.animator;

#nullable enable

public class LerpAnimator<T> : IAnimator 
where T : 
  IFloatingPoint<T>
{

  private readonly IMemberWrap<T> target;
  private readonly T from;
  private readonly T to;

  private readonly ILogger logger = LoggerSingleton.GetLogger();
  private readonly EasingFunction easing;

  public LerpAnimator(
    IMemberWrap<T> target,
    T from,
    T to,
    EasingFunction easing
  ) {
    this.easing = easing;
    this.target = target;
    this.from = from;
    this.to = to;
  }

  public void Mix(float t) {
    T time_c = T.CreateSaturating(easing(Math.Clamp(t, 0.0, 1.0)));
    T result = (from * (T.CreateSaturating(1.0) - time_c)) + (to * time_c);
    target.SetValue(result);
  }
}