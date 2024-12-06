namespace digipet.diary;

public interface ITextEditor {
  // returns char offset
  int Cursor {
    get; set;
  }

  // returns this editor's contents as a string
  public string AsString();

  // put single char or string, moves cursor to end
  public void Put(char c);
  public void Put(string s);

  public int Length();

  // delete @ cursor
  public void Delete();

  // clears editor content
  public void Clear();

  // saves this editor state
  public void Save();

  // deletes this editor state

  public void Erase();
}