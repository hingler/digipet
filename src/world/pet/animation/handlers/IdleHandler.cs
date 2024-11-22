using digipet.framework;
using digipet.image;

namespace digipet.world.pet.animation.handlers;

public class IdleHandler : IAnimationHandler {
  private readonly IAnimationData data_idle;

  // streamline
  public IdleHandler(IEngine engine) : this(engine, 0.5, PetAnimation.IDLE) {}
  public IdleHandler(IEngine engine, double frame_time, PetAnimation anim) {
    AnimationDataBuilder builder = new();
    
    IAnimatedSprite sprite = engine.GetSpriteFetcher().GetAnimatedSprite(digipet.sprite.attrib.SpriteID.SPRITE_PET);
    sprite.HFrames = 2;
    sprite.VFrames = 1;

    builder.WithAnimatedSprite(sprite, frame_time, anim, true);
    builder.WithFaceOffset(1, new(0, 1));

    data_idle = builder.Build();
  }

  public IAnimationData GetAnimationData(PetAnimation animation) {
    return data_idle;
  }
}