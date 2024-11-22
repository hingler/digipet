using digipet.file.stream;
using digipet.sim;


public class PetData : IPetData, IStreamable {
  public double Food { get; }
  public double Water { get; }
  public double Fun { get; }
  public double Social { get; }
  public double Energy { get; }
  public long PetExp { get; }
  public string PetName { get; }

  public PetData() {}
  public PetData(
    double food, 
    double water, 
    double fun, 
    double social, 
    double energy, 
    long petExp,
    string petName
  ) {
    Food = food;
    Water = water;
    Fun = fun;
    Social = social;
    Energy = energy;
    PetExp = petExp;
    PetName = petName;
  }

  public PetData(IPetData src) : this(
    src.Food,
    src.Water, 
    src.Fun, 
    src.Social, 
    src.Energy, 
    src.PetExp, 
    src.PetName
  ) {}

  public PetData(IInputStream stream) : this(
    stream.ReadDouble(),
    stream.ReadDouble(),
    stream.ReadDouble(),
    stream.ReadDouble(),
    stream.ReadDouble(),
    stream.ReadInt64(),
    stream.ReadPascalString()
  ) {}

  public void ToStream(IOutputStream stream) {
    stream.WriteDouble(Food);
    stream.WriteDouble(Water);
    stream.WriteDouble(Fun);
    stream.WriteDouble(Social);
    stream.WriteDouble(Energy);
    stream.WriteInt64(PetExp);
    stream.WritePascalString(PetName);
  }
}