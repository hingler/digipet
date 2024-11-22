using digipet.file.stream;

namespace digipet.sim.edible;

public class TasteData : IStreamable {
  public readonly double Taste;

  public TasteData(double taste) { Taste = taste; }

  public TasteData(IInputStream stream) {
    Taste = stream.ReadDouble();
  }

  public void ToStream(IOutputStream stream) {
    stream.WriteDouble(Taste);
  }
}