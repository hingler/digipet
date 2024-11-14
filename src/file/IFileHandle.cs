
using digipet.file.stream;

namespace digipet.file;

public interface IFileHandle {
  // return this file as string
  string AsString();

  string GetLine();
  // this is all i care for rn - might need more later

  // true if eof reached

  void Seek(ulong pos);
  ulong Pos();
  bool Eof();

  byte[] ReadBytes(long byte_count);
  void WriteBytes(byte[] bytes);
  long GetByteCount();
}