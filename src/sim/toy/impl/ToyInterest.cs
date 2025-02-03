using digipet.file.stream;

namespace digipet.sim.toy.impl;

public class ToyInterest : IStreamable {
  // initial enjoyment
  public double BaseEnjoyment;

  // number of ticks to 0
  public double RegenRate;

  // current enjoyment value
  public double CurrentEnjoyment;

  public ToyInterest() {}

  public ToyInterest(IInputStream stream) {
    BaseEnjoyment = stream.ReadDouble();
    RegenRate = stream.ReadDouble();
    CurrentEnjoyment = stream.ReadDouble();
  }

  public void ToStream(IOutputStream stream) {
    stream.WriteDouble(BaseEnjoyment);
    stream.WriteDouble(RegenRate);
    stream.WriteDouble(CurrentEnjoyment);
  }
}