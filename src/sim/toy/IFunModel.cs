namespace digipet.sim.toy;

public interface IFunModel : ISimComponent {
  // convert to streamable data
  
  // the fun model is more of a passive thing
  // we take in physworld data (ie which toys are active) and just pull a bit of enjoyment from each

  double Fun { get; set; }

  // everything else is passive! (ie: crunch it ourselves, every tick)
}