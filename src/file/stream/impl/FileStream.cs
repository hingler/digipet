using System.Text;

namespace digipet.file.stream.impl;

public class FileStream : IInOutStream {
  private readonly IFileHandle handle;

  public FileStream(IFileHandle handle) {
    this.handle = handle;
    this.handle.Seek(IStream.Begin);
  }

  public void Seek(ulong pos) {
    handle.Seek(pos);
    
  }

  public ulong Pos() => handle.Pos();
  public void WriteBool(bool data) => handle.WriteBytes(BitConverter.GetBytes(data));
  public void WriteChar(char data) => handle.WriteBytes(BitConverter.GetBytes(data));
  public void WriteInt32(int data) => handle.WriteBytes(BitConverter.GetBytes(data));
  public void WriteInt64(long data) => handle.WriteBytes(BitConverter.GetBytes(data));
  public void WriteFloat(float data) => handle.WriteBytes(BitConverter.GetBytes(data));
  public void WriteDouble(double data) => handle.WriteBytes(BitConverter.GetBytes(data));
  public void WritePascalString(string data) {
    WriteInt32(data.Length);
    handle.WriteBytes(Encoding.UTF8.GetBytes(data));
  }

  public bool ReadBool() => BitConverter.ToBoolean(handle.ReadBytes(sizeof(bool)));
  public char ReadChar() => BitConverter.ToChar(handle.ReadBytes(sizeof(char)));
  public int ReadInt32() => BitConverter.ToInt32(handle.ReadBytes(sizeof(int)));
  public long ReadInt64() => BitConverter.ToInt64(handle.ReadBytes(sizeof(long)));
  public float ReadFloat() => BitConverter.ToSingle(handle.ReadBytes(sizeof(float)));
  public double ReadDouble() => BitConverter.ToDouble(handle.ReadBytes(sizeof(double)));
  public string ReadPascalString() {
    int len = ReadInt32();
    return Encoding.UTF8.GetString(handle.ReadBytes(len));
  }

  public string ReadString(long byte_count) => Encoding.UTF8.GetString(handle.ReadBytes(byte_count));
  public byte[] ReadBytes(long byte_count) => handle.ReadBytes(byte_count);
}