using System.Numerics;
using digipet.sim;

namespace digipet.world.pet;

// object representing the pet state
public interface IPetObject {
  // position of this obj
  public Vector2 Position { get; }
}