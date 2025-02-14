using System.Collections.ObjectModel;
using digipet.file.stream;

namespace digipet.sim.toy.impl;

public class ToyModelData : IStreamable {
  public readonly ReadOnlyDictionary<int, ToyInterest> toys;

  public ToyModelData() : this([]) {}

  public ToyModelData(Dictionary<int, ToyInterest> toys) {
    this.toys = toys.AsReadOnly();
  }

  public ToyModelData(IInputStream stream) {
    Dictionary<int, ToyInterest> toys = [];
    int toy_count = stream.ReadInt32();

    for (int i = 0; i < toy_count; i++) {
      int rid = stream.ReadInt32();
      ToyInterest interest = new(stream);

      toys[rid] = interest;
    }

    this.toys = toys.AsReadOnly();
  }

  public void ToStream(IOutputStream stream) {
    stream.WriteInt32(toys.Count);
    foreach (KeyValuePair<int, ToyInterest> pair in toys) {
      stream.WriteInt32(pair.Key);
      pair.Value.ToStream(stream);
    }
  }
}