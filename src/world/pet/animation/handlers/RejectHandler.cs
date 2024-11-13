using System.Numerics;
using digipet.framework;
using digipet.image;
using digipet.sprite.attrib;

namespace digipet.world.pet.animation.handlers;

public class RejectHandler : IAnimationHandler {
  private readonly IAnimationData data_reject;

  public RejectHandler(IEngine engine) {
    AnimationDataBuilder builder = new();

    IAnimatedSprite sprite = engine.GetSpriteFetcher().GetAnimatedSprite(
      SpriteID.SPRITE_PET
    );

    sprite.HFrames = 2;
    sprite.VFrames = 1;
    sprite.Frame = 0;

    builder.WithFrames(
      [ sprite, sprite ],
      PetAnimation.REJECT,
      0.5,
      false
    );

    builder.WithFaceOffset(0, new(-1, 0));
    builder.WithFaceOffset(1, Vector2.Zero);

    data_reject = builder.Build();
  }

  public IAnimationData GetAnimationData(PetAnimation animation) {
    return data_reject;
  }
}