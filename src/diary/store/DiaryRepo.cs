using digipet.file.stream;

namespace digipet.diary.store;

public class DiaryRepo : IStreamable, IDiaryRepo {
  public SortedSet<StreamableDiaryRecord> records;

  public DiaryRepo() {
    records = new();
  }

  public DiaryRepo(IInputStream stream) : this() {
    int count = stream.ReadInt32();
    for (int i = 0; i < count; i++) {
      records.Add(new StreamableDiaryRecord(stream));
    }
  }

  // some sort of key to disambiguate??
  public void AddRecord(string s) {
    StreamableDiaryRecord record = new(s);
    records.Add(record);
  }

  public IEnumerable<IDiaryRecord> GetRecords() {
    return records.Cast<IDiaryRecord>();
  }

  public void ToStream(IOutputStream stream) {
    stream.WriteInt32(records.Count);
    foreach (StreamableDiaryRecord record in records) {
      record.ToStream(stream);
    }
  }
}