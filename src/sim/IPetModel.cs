namespace digipet.sim;

// this works for now
// thinking: we'll come up with some better way to pass in items, rather than modifying stats ourselves
public interface IPetModel {
  double Food { get; }
  double Water { get; }
  double Fun { get; }
  double Social { get; }
  double Energy { get; }

  long PetExp { get; }
  string PetName { get; }
}