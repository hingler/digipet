using digipet.framework;

namespace digipet.world.pet.animation.handlers;

public class PauseHandler : IAnimationHandler {
  private readonly IAnimationHandler underlying;

  public PauseHandler(IEngine engine) {
    underlying = new IdleHandler(engine, 9999.0, PetAnimation.PAUSE);
  }

  public IAnimationData GetAnimationData(PetAnimation animation) {
    return underlying.GetAnimationData(animation);
  }
}