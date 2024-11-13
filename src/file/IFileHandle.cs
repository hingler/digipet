
namespace digipet.file;

public interface IFileHandle {
  // return this file as string
  string AsString();

  string GetLine();
  // this is all i care for rn - might need more later

  // true if eof reached
  bool Eof();

  // file write if open
  // writes to end of file
  void Write(string content);
}