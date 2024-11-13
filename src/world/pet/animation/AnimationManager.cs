using System.Collections;
using System.Collections.Generic;

namespace digipet.world.pet.animation;

public class AnimationManager() {
  private readonly IDictionary<PetAnimation, IAnimationHandler> handlers = 
    new Dictionary<PetAnimation, IAnimationHandler>();

  public void AddHandler(PetAnimation animation, IAnimationHandler handler) {
    handlers[animation] = handler;
  }

  public IAnimationData GetAnimationData(PetAnimation animation) {
    return handlers[animation].GetAnimationData(animation);
  }
}