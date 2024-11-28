namespace digipet.diary;

public interface ITextEditor {
  // returns char offset
  int Cursor {
    get; set;
  }

  // returns this editor's contents as a string
  public string AsString();

  // put single char or string
  public void Put(char c);
  public void Put(string s);

  // delete @ cursor
  public void Delete();
}