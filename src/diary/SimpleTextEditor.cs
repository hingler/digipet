using System.Text;
using digipet.util;

namespace digipet.diary;

#nullable enable

public class SimpleTextEditor : ITextEditor {
  private readonly StringBuilder buffer;

  // tba: do some x2 checking here

  private int cursor_;
  public int Cursor {
    get => cursor_;
    set {
      cursor_ = Math.Clamp(value, 0, buffer.Length);
    }
  }


  public SimpleTextEditor() {
    buffer = new();
    cursor_ = 0;
  }

  public WordResult GetNextWord() {
    // cursor refers to char in front
    // march forward til we hit a non space char
    // then, march forward again until we hit a space
    // reverse 1, to undo the space

    int start_index = FindNextNonSpace(cursor_);
    int end_index = FindNextSpace(start_index);

    return GetWordResult(start_index, end_index);
  }

  public WordResult GetPreviousWord() {
    int end_index = FindPrevNonSpace(cursor_ - 1);
    int start_index = FindPrevSpace(end_index) + 1;

    return GetWordResult(start_index, end_index);

  }

  private WordResult GetWordResult(int start_index, int end_index) {
    string? word = null;

    this.GetLogger().Log("from ", start_index, " to ", end_index);
    if (start_index < end_index) {
      word = CollateBuffer(start_index, end_index);
    }

    return new() {
      StartIndex = start_index,
      Word = word
    };
  }

  private delegate bool FinderFunc(int index);

  private int GetCharIndex(int start_index, bool seek_forwards, bool find_space) {
    int cursor_search = start_index;
    int seek_inc = (seek_forwards ? 1 : -1);
    FinderFunc is_char = (find_space ? IsSpace : IsNonSpace);
    while (
      (cursor_search >= 0) && (cursor_search < buffer.Length) &&
      !is_char(cursor_search)
    ) {
      cursor_search += seek_inc;
    }

    return cursor_search;
  }

  private bool IsSpace(int index) => char.IsWhiteSpace(buffer[index]);
  private bool IsNonSpace(int index) => !(char.IsWhiteSpace(buffer[index]));

  private int FindNextSpace(int index) => GetCharIndex(index, true, true);
  private int FindNextNonSpace(int index) => GetCharIndex(index, true, false);
  private int FindPrevSpace(int index) => GetCharIndex(index, false, true);
  private int FindPrevNonSpace(int index) => GetCharIndex(index, false, false);

  private string CollateBuffer(int start_index, int end_index) {
    string s = "";
    for (int c = start_index; c < end_index; c++) {
      s += buffer[c];
    }

    return s;
  }

  public int Length() {
    return buffer.Length;
  }

  public void Put(char c) {
    buffer.Insert(Cursor++, c);
  }

  public void Put(string s) {
    buffer.Insert(Cursor, s);
    Cursor += s.Length;
  }

  public string AsString() {
    return buffer.ToString();
  }

  public void Delete() {
    if (cursor_ > 0) {
      this.GetLogger().Log("deleting char: ", (int)buffer[Cursor - 1]);
      buffer.Remove(--Cursor, 1);
    }
  }

  public void Clear() {
    buffer.Clear();
    cursor_ = 0;
  }

  public void Save() { /* no op */ }
  public void Erase() { /* no op */ }
}