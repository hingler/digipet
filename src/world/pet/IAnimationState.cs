using System.Numerics;
using digipet.image;
using digipet.pet;

namespace digipet.world.pet;

public enum PetAnimation {
  IDLE,
  PAUSE,
  EXPRESS,
  ACTIVE,
  REJECT,
}


// how do we want to feed this back?
// - animation state should just be a frame time, an array of frames, and a step time
// - the rest is irrelevant
// struct representing current animation state
public interface IAnimationState {
  // animation being performed
  public PetAnimation CurrentAnimation { get; }
  
  // current frame of this animation
  public int Frame { get; }

  // number of frames this animation is expected to span
  public int FrameCount { get; }

  // runtime of this animation
  public double Duration { get; }
  public double FrameDuration { get => Duration / FrameCount; }

  // current play time for this animation
  public double Progress { get; }
  
  // true if animation is looping, false otherwise
  public bool Loop { get; }
  public Vector2 FaceOffsetPx { get; }


  // metadata?? (ex. which frame to do (xyz) on)
  public ISprite Sprite { get; }
}