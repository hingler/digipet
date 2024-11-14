using System.Text;
using digipet.util;

namespace digipet.file.stream.impl;

public class ByteStream : IInOutStream {
  private readonly byte[] bytes;
  private ulong cursor;

  public ByteStream(IInputStream stream, long bytes) : this(stream.ReadBytes(bytes)) {}

  public ByteStream(byte[] bytes) {
    this.bytes = bytes;
    cursor = 0;
  }

  public void Seek(ulong pos) {
    cursor = Math.Clamp(pos, 0, (ulong)bytes.Length);
  }

  public ulong Pos() => cursor;
  public bool ReadBool() => BitConverter.ToBoolean(bytes, (int)cursor++);
  public char ReadChar() {
    char res = BitConverter.ToChar(bytes, (int)cursor);
    cursor += sizeof(char);
    return res;
  }
  public int ReadInt32() {
    int res = BitConverter.ToInt32(bytes, (int)cursor);
    cursor += sizeof(int);
    
    return res;
  } 

  public long ReadInt64() {
    long res = BitConverter.ToInt64(bytes, (int)cursor);
    cursor += sizeof(long);
    return res;
  }

  public float ReadFloat() {
    float res = BitConverter.ToSingle(bytes, (int)cursor);
    cursor += sizeof(float);
    return res;
  }

  public double ReadDouble() {
    double res = BitConverter.ToDouble(bytes, (int)cursor);
    cursor += sizeof(double);
    return res;
  }

  public string ReadPascalString() {
    int len = ReadInt32();
    return ReadString(len);
  }

  public string ReadString(long byte_count) {
    string res = Encoding.UTF8.GetString(bytes, (int)cursor, (int)byte_count);
    cursor += (ulong)byte_count;
    return res;
  }

  public byte[] ReadBytes(long byte_count) {
    byte[] res = new byte[byte_count];
    Array.Copy(bytes, (int)cursor, res, 0, (int)byte_count);
    cursor += (ulong)byte_count;
    return res;
  }

  private void Overwrite(byte[] data) {
    for (int i = 0; i < data.Length; i++) {
      bytes[cursor++] = data[i];
    }
  }

  public void WriteBool(bool data) => Overwrite(BitConverter.GetBytes(data));
  public void WriteChar(char data) => Overwrite(BitConverter.GetBytes(data));
  public void WriteInt32(int data) => Overwrite(BitConverter.GetBytes(data));
  public void WriteInt64(long data) => Overwrite(BitConverter.GetBytes(data));
  public void WriteFloat(float data) => Overwrite(BitConverter.GetBytes(data));
  public void WriteDouble(double data) => Overwrite(BitConverter.GetBytes(data));
  public void WritePascalString(string data) {
    WriteInt32(data.Length);
    Overwrite(Encoding.UTF8.GetBytes(data));
  }
}