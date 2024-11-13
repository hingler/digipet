using System.Collections.Generic;
using System.Numerics;
using digipet.image;

namespace digipet.world.pet.animation;

public class AnimationDataBuilder : IAnimationData {
  private IAnimationData underlying_data;
  private IList<Vector2> frame_offsets = [];

  public AnimationDataBuilder() {
    underlying_data = null;
  }

  public AnimationDataBuilder WithFrames(
    IList<ISprite> frames, 
    PetAnimation animation,
    double frame_time, 
    bool loop
  ) {
    AnimationData anim_frames = new(animation, frame_time, loop);
    foreach (ISprite sprite in frames) {
      anim_frames.AddSprite(sprite);
    }

    underlying_data = anim_frames;
    return this;
  }

  public AnimationDataBuilder WithAnimatedSprite(
    IAnimatedSprite sprite,
    double frame_time,
    PetAnimation animation,
    bool loop
  ) {
    underlying_data = new AnimatedSpriteData(
      sprite, frame_time, animation, loop
    );

    return this;
  }

  public AnimationDataBuilder WithFaceOffset(
    int frame, Vector2 offset
  ) {
    while (frame_offsets.Count <= frame) {
      frame_offsets.Add(Vector2.Zero);
    }

    frame_offsets[frame] = offset;
    return this;
  }

  public IAnimationData Build() {
    return this;
  }

  public PetAnimation CurrentAnimation => underlying_data.CurrentAnimation;
  public int Frame => underlying_data.Frame;
  public int FrameCount => underlying_data.FrameCount;
  public double Duration => underlying_data.Duration;
  public double Progress => underlying_data.Progress;
  public bool Loop => underlying_data.Loop;

  public Vector2 FaceOffsetPx {
    get {
      if (frame_offsets.Count > Frame) {
        return frame_offsets[Frame];
      } else if (frame_offsets.Count > 0) {
        return frame_offsets[0];
      }

      return Vector2.Zero;
    }
  }

  public bool Complete() {
    return underlying_data.Complete();
  }

  public ISprite GetCurrentSprite() {
    return underlying_data.GetCurrentSprite();
  }

  public void Reset() {
    underlying_data.Reset();
  }

  public void Update(double delta) {
    underlying_data.Update(delta);
  }
}