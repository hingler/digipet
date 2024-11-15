using digipet.file.stream;

namespace digipet.sim.edible;

public class TasteData : IStreamable<TasteData> {
  public readonly double Taste;

  public TasteData(double taste) { Taste = taste; }

  public static TasteData FromStream(IInputStream stream) {
    return new TasteData(stream.ReadDouble());
  }

  public void ToStream(IOutputStream stream) {
    stream.WriteDouble(Taste);
  }
}