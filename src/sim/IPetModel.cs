using digipet.file.stream;
using digipet.pet;
using digipet.sim.edible;
using digipet.sim.impl;
using digipet.sim.water;

namespace digipet.sim;

public interface IPetData {
  double Food { get; }
  double Water { get; }
  double Fun { get; }
  double Social { get; }
  double Energy { get; }
  long PetExp { get; }
  string PetName { get; }
  IPetPersonality Personality { get; }
}

public struct PetDataParcel : IPetData, IStreamable {
  public double Food { get; set; } = 0.5;
  public double Water { get; set; } = 0.5;
  public double Fun { get; set; } = 0.5;
  public double Social { get; set; } = 0.5;
  public double Energy { get; set; } = 0.5;
  public long PetExp { get; set; } = 0;
  public string PetName { get; set; } = "Dingus";

  public IPetPersonality Personality { get; set; }

  public PetDataParcel() {
    Food = 0.5;
    Water = 0.5;
    Fun = 0.5;
    Social = 0.5;
    Energy = 0.5;
    PetExp = 0;
    PetName = "PLACEHOLDER";
    Personality = new PetPersonalityData();
  }

  public PetDataParcel(IPetData other) {
    Food = other.Food;
    Water = other.Water;
    Fun = other.Fun;
    Social = other.Social;
    Energy = other.Energy;
    PetExp = other.PetExp;
    PetName = other.PetName;
    Personality = other.Personality;
  }

  public PetDataParcel(IInputStream stream) {
    Food = stream.ReadDouble();
    Water = stream.ReadDouble();
    Fun = stream.ReadDouble();
    Social = stream.ReadDouble();
    Energy = stream.ReadDouble();
    PetExp = stream.ReadInt64();
    PetName = stream.ReadPascalString();
    Personality = new PetPersonalityData(stream);
  }

  public void ToStream(IOutputStream stream) {
    stream.WriteDouble(Food);
    stream.WriteDouble(Water);
    stream.WriteDouble(Fun);
    stream.WriteDouble(Social);
    stream.WriteDouble(Energy);
    stream.WriteInt64(PetExp);
    stream.WritePascalString(PetName);

    // tba: validate byte width??

    PetPersonalityData personality = new(Personality);
    personality.ToStream(stream);
  }
}

// this works for now
// thinking: we'll come up with some better way to pass in items, rather than modifying stats ourselves
public interface IPetModel : IPetData, ISimComponent {

  bool CanEat(IEdiblePickup food_item);
  // eats the food item and returns a desirability score
  // 0.0 is avg - + is good, - is bad
  bool TryEat(IEdiblePickup food_item, out double score);
  double Drink(double units, IWaterSource source);

  IPetData AsPetData() {
    return new PetData(this);
  }
}