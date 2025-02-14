namespace digipet.diary.store;

public class RepoEditor : ITextEditor {
  private readonly IDiaryRepo repo;

  private readonly SimpleTextEditor underlying;

  public int Cursor {
    get => underlying.Cursor;
    set => underlying.Cursor = value;
  }

  public RepoEditor(IDiaryRepo dest) {
    repo = dest;
    underlying = new();
  }

  public WordResult GetNextWord() => underlying.GetNextWord();
  public WordResult GetPreviousWord() => underlying.GetPreviousWord();
  public string AsString() => underlying.AsString();
  public void Put(char c) => underlying.Put(c);
  public void Put(string s) => underlying.Put(s);
  public int Length() => underlying.Length();
  public void Delete() => underlying.Delete();
  public void Clear() => underlying.Clear();
  public void Save() {
    underlying.Save();
    repo.AddRecord(AsString());
  }

  public void Erase() { /* no op atm */  }
}