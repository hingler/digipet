namespace digipet.sprite;

public class FrameTicker(double frame_time, int frame_count) {
  private double delta_acc = 0.0;
  private int frame = 0;

  public double FrameTime = frame_time;
  public int FrameCount = frame_count;


  public FrameTicker() : this(0.5f, 1) {}

  public bool Tick(double delta) {
    delta_acc += delta;
    bool clip = delta_acc >= FrameTime;
    if (clip) {
      delta_acc %= FrameTime;
      frame = (frame + 1) % FrameCount;
    }

    return clip;
  }

  public int Frame => frame;
}