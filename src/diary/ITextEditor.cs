namespace digipet.diary;

// string: next word
// int: start index of said word

#nullable enable

public struct WordResult {
  // null if not a word (ex when seeking back from first word, or seeking forward from last)
  public string? Word;
  public int StartIndex;

  public readonly int EndIndex => StartIndex + (Word?.Length ?? 0);
}

public interface ITextEditor {
  // returns char offset
  int Cursor {
    get; set;
  }

  public WordResult GetNextWord();
  public WordResult GetPreviousWord();

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