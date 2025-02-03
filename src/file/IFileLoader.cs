using digipet.image;

namespace digipet.file;

public enum FileFlags {
  Read = 1,
  Write = 2
}

// diff loaders for userdata and resourcedata
public interface IFileLoader {
  // r, w, a
  IFileHandle Load(string path, FileFlags flags);
  ISprite LoadSprite(string path);
  IAnimatedSprite LoadAnimatedSprite(string path);
  bool Exists(string path);
}