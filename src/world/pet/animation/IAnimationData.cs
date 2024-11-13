using System;
using System.Collections.Generic;
using digipet.image;

namespace digipet.world.pet.animation;

public interface IAnimationData : IAnimationState {
  public ISprite GetCurrentSprite();
  ISprite IAnimationState.Sprite { get => GetCurrentSprite(); }
  void Update(double delta);

  // reset the animation back to initial state
  void Reset();

  bool Complete();

}