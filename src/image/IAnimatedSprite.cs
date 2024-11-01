namespace digipet.image;

public interface IAnimatedSprite : ISprite {
  // return currently displayed frame
  int Frame { get; set; }

  int HFrames { get; set; }
  int VFrames { get; set; }

  // get total frame count
  int GetFrameCount();

  void Increment() {
    Frame = (Frame + 1) % GetFrameCount();
  }
}