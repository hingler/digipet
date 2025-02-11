using digipet.file.stream;

namespace digipet.sim.toy.impl;

public class ToyInterest : IStreamable {
  // initial enjoyment
  public double BaseEnjoyment;

  // number of ticks to max
  public double RegenRate;
  public double ConsumeRate;

  // current enjoyment value
  public double CurrentEnjoyment;

  public ToyInterest() {}

  public ToyInterest(IInputStream stream) {
    BaseEnjoyment = stream.ReadDouble();
    RegenRate = stream.ReadDouble();
    ConsumeRate = stream.ReadDouble();
    CurrentEnjoyment = stream.ReadDouble();
  }

  public void ToStream(IOutputStream stream) {
    stream.WriteDouble(BaseEnjoyment);
    stream.WriteDouble(RegenRate);
    stream.WriteDouble(ConsumeRate);
    stream.WriteDouble(CurrentEnjoyment);
  }
}