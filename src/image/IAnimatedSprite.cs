using digipet.sprite;

namespace digipet.image;

public interface IAnimatable {
  int Frame { get; set; }
  int GetFrameCount();
}

public interface IAnimatedSprite : ISprite, IAnimatable {
  int HFrames { get; set; }
  int VFrames { get; set; }

  void Increment() {
    Frame = (Frame + 1) % GetFrameCount();
  }
}

public interface ISpriteSequence : ISprite, IAnimatable {}