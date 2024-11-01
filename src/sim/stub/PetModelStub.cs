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
}