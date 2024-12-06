namespace digipet.diary.store;

public interface IDiaryRecord {
  DateTime CreationTime { get; }
  string Content { get; }
}