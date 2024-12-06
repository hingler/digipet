namespace digipet.diary.store;

public interface IDiaryRepo {
  public void AddRecord(string s);
  public IEnumerable<IDiaryRecord> GetRecords();
}