namespace digipet.sim.energy;

public interface IEnergyModel : ISimComponent {
  double Energy { get; set; }
}