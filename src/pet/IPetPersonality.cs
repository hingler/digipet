using digipet.file.stream;

namespace digipet.pet;

// si9mple sketch for personality types
public interface IPetPersonality {
  public double Charm { get; }
  public double Tough { get; }
  public double Brave { get; }
  public double Smart { get; }
  public double Quick { get; }
  public double Weird { get; }
}

public struct PetPersonalityData : IPetPersonality, IStreamable {
  public double Charm { get; set; }
  public double Tough { get; set; }
  public double Brave { get; set; }
  public double Smart { get; set; }
  public double Quick { get; set; }
  public double Weird { get; set; }

  public PetPersonalityData() {
    Charm = 0.5;
    Tough = 0.5;
    Brave = 0.5;
    Smart = 0.5;
    Quick = 0.5;
    Weird = 0.5;
  }

  public PetPersonalityData(IPetPersonality other) {
    Charm = other.Charm;
    Tough = other.Tough;
    Brave = other.Brave;
    Smart = other.Smart;
    Quick = other.Quick;
    Weird = other.Weird;
  }

  public PetPersonalityData(IInputStream stream) {
    Charm = stream.ReadDouble();
    Tough = stream.ReadDouble();
    Brave = stream.ReadDouble();
    Smart = stream.ReadDouble();
    Quick = stream.ReadDouble();
    Weird = stream.ReadDouble();
  }

  public readonly void ToStream(IOutputStream stream) {
    stream.WriteDouble(Charm);
    stream.WriteDouble(Tough);
    stream.WriteDouble(Brave);
    stream.WriteDouble(Smart);
    stream.WriteDouble(Quick);
    stream.WriteDouble(Weird);
  }
}