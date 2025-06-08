using System.Numerics;
using digipet.framework;
using digipet.image;
using digipet.sprite.attrib;

namespace digipet.world.pet.animation.handlers;

public class SleepHandler : IAnimationHandler {
  private readonly IAnimationData data_sleep;

  public SleepHandler(IEngine engine) {
    AnimationDataBuilder builder = new();

    IAnimatedSprite sprite = engine.GetSpriteFetcher().GetAnimatedSprite(
      SpriteID.SPRITE_PET
    );

    sprite.HFrames = 2;
    sprite.VFrames = 1;
    sprite.Frame = 0;

    builder.WithAnimatedSprite(
      sprite, 
      1.0, 
      PetAnimation.SLEEP,
      true
    );

    builder.WithFaceOffset(0, Vector2.Zero);
    builder.WithFaceOffset(1, new(0, 1));

    data_sleep = builder.Build();
  }

  public IAnimationData GetAnimationData(PetAnimation animation) {
    return data_sleep;
  }
}