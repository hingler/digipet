namespace digipet.sprite;

public class FrameTicker(double frame_time, int frame_count) {
  private double delta_acc = 0.0;
  private int frame = 0;

  public bool Tick(double delta) {
    delta_acc += delta;
    bool clip = delta_acc >= frame_time;
    if (clip) {
      delta_acc %= frame_time;
      frame = (frame + 1) % frame_count;
    }

    return clip;
  }

  public int Frame => frame;
}