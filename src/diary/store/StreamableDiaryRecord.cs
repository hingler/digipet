using digipet.file.stream;

namespace digipet.diary.store;

#nullable enable

public class StreamableDiaryRecord : IDiaryRecord, IStreamable, IComparable<StreamableDiaryRecord> {
  public DateTime CreationTime { get; }
  public string Content { get; }

  public StreamableDiaryRecord(string content) : this(content, DateTime.Now) {}

  public StreamableDiaryRecord(string content, DateTime creation_time) {
    CreationTime = creation_time;
    Content = content;
  }

  public StreamableDiaryRecord(IInputStream stream) {
    long tick_count = stream.ReadInt64();
    CreationTime = new DateTime(tick_count);
    Content = stream.ReadPascalString();
  }

  public void ToStream(IOutputStream stream) {
    stream.WriteInt64(CreationTime.Ticks);
    stream.WritePascalString(Content);
  }

  public int CompareTo(StreamableDiaryRecord? other) {
    return CreationTime.CompareTo(other?.CreationTime);
  }
}