using System.Collections;
using System.Collections.Generic;
using digipet.framework;
using digipet.world.pet.animation.handlers;

namespace digipet.world.pet.animation;

public class AnimationManager(IEngine engine) {
  private readonly IDictionary<PetAnimation, IAnimationHandler> handlers = 
    new Dictionary<PetAnimation, IAnimationHandler>();

  private readonly IdleHandler default_handler = new(engine);

  public void AddHandler(PetAnimation animation, IAnimationHandler handler) {
    handlers[animation] = handler;
  }

  public IAnimationData GetAnimationData(PetAnimation animation) {
    if (handlers.TryGetValue(animation, out IAnimationHandler handler)) {
      return handler.GetAnimationData(animation);
    }

    return default_handler.GetAnimationData(animation);
  }
}