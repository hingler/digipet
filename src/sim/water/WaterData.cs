using digipet.file.stream;
using digipet.sim.water;

namespace digipet.sim.water;

public class WaterData : IWaterData, IStreamable<WaterData> {
  public double Capacity { get; }
  public double Contents { get; }

  public WaterData(IWaterData data) : this(data.Capacity, data.Contents) {}

  public WaterData(double Capacity, double Contents) {
    this.Capacity = Capacity;
    this.Contents = Contents;
  }

  public static WaterData FromStream(IInputStream stream) {
    return new WaterData(
      stream.ReadDouble(),
      stream.ReadDouble()
    );
  }

  public void ToStream(IOutputStream stream) {
    stream.WriteDouble(Capacity);
    stream.WriteDouble(Contents);
  }

}