using digipet.file.stream;

namespace digipet.db;

public class StreamableLong : IStreamable {
  public readonly long data;
  public StreamableLong(long data) {
    this.data = data;
  }

  public StreamableLong(IInputStream stream) {
    data = stream.ReadInt64();
  }

  public void ToStream(IOutputStream stream) {
    stream.WriteInt64(data);
  }
}