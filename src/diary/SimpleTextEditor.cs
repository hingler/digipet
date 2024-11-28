using System.Security.Cryptography;
using System.Text;
using digipet.util;

namespace digipet.diary;

public class SimpleTextEditor : ITextEditor {
  private readonly StringBuilder buffer;
  private static readonly ILogger logger = LoggerSingleton.GetStaticLogger<SimpleTextEditor>();

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
}