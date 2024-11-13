using digipet.sim.edible;
using digipet.sim.water;

namespace digipet.sim.stub;

public class PetModelStub : IPetModel
{
  public double Food => 0.75;

  public double Water => 0.5;

  public double Fun => 0.8;

  public double Social => 0.5;

  public double Energy => 0.3;

  public long PetExp => 128;

  public string PetName => "DINGUS";

  public bool CanEat(IEdiblePickup food_item) {
    return true;
  }

  public bool TryEat(IEdiblePickup food_item, out double score) {
    score = 0.0;
    return true;
  }

  public double Drink(double units, IWaterSource source) {
    return 0.0;
  }
}