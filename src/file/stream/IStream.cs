namespace digipet.file.stream;

public interface IStream {
  public static readonly ulong Begin = 0;
  public static readonly ulong End = long.MaxValue;
  void Seek(ulong pos);

  // return End if at end of file
  ulong Pos();
}

public interface IInputStream : IStream {
  bool ReadBool();
  char ReadChar();
  int ReadInt32();
  long ReadInt64();
  float ReadFloat();
  double ReadDouble();

  // reads string, assuming next 4 bytes contain the length of the string, and following the chars
  string ReadPascalString();
  string ReadString(long byte_count);

  byte[] ReadBytes(long byte_count);
}

public interface IOutputStream : IStream {
  void WriteBool(bool data);
  void WriteChar(char data);
  void WriteInt32(int data);
  void WriteInt64(long data);
  void WriteFloat(float data);
  void WriteDouble(double data);

  // writes length as an int32, then writes utf-16 string char by char
  void WritePascalString(string content);
}

public interface IInOutStream : IStream, IInputStream, IOutputStream {}