namespace digipet.image;

#nullable enable

// base sequence class
public class EngineSpriteSequence<T> : IAnimatable where T : ISprite {
  private readonly List<T> sprites;

  public EngineSpriteSequence() {
    sprites = [];
  }

  public void AddSprite(T sprite) {
    sprites.Add(sprite);
  }

  public int frame_ = 0;

  public int Frame {
    get => frame_;
    set {
      if (sprites.Count <= 0) {
        frame_ = 0;
      } else {
        frame_ = Math.Clamp(value, 0, sprites.Count - 1);
      }
    } 
  }

  public int GetFrameCount() => sprites.Count;
  public T? GetSprite() => sprites.Count > 0 ? sprites[Frame] : default;
}