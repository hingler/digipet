using digipet.world.pet;

namespace digipet.util;

public class AnimationTicker() {

  private bool delta_func = false;

  public bool Update(IAnimationState data, double delta) {
    if (data == null) {
      return false;
    }
    
    double frame_duration = data.FrameDuration;
    double frame_time = data.Progress % frame_duration;

    if ((frame_time + delta > frame_duration) && !delta_func) {
      delta_func = true;
      return true;
    } else if ((frame_time + delta < frame_duration) && delta_func) {
      delta_func = false;
    }

    return false;
  }
}