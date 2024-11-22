namespace digipet.file.stream;

public interface IStreamable {

  // creates an instance from the stream's contents
  // (we do typecasting anyway so doesnt rly matter)
  void ToStream(IOutputStream stream);
}