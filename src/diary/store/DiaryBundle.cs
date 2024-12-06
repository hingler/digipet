namespace digipet.diary.store;

public class DiaryBundle {
  public readonly IList<IDiaryRecord> entries = [];

  public DiaryBundle() {}

  public void AddEntry(IDiaryRecord record) {
    entries.Add(record);
  }

  public IReadOnlyList<IDiaryRecord> GetRecords() {
    return entries.AsReadOnly();
  }
}