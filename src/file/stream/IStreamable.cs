namespace digipet.file.stream;

// classes which can be read/written from/to streams

public interface IOutStreamable {
  void ToStream(IOutputStream stream);
}

public interface IStreamable<T> : IOutStreamable where T : IStreamable<T> {

  // creates an instance of T from the stream's contents
  static abstract T FromStream(IInputStream stream);
}