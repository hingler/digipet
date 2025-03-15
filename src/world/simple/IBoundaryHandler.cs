using System.Numerics;

namespace digipet.world.simple;

// resolves positions of any objects crossing some boundary
public interface IBoundaryHandler {
  // given an object, return a position delta and a velocity delta indicating correction factor
  CollisionData HandleBoundaries(
    IPhysObject o, 
    float bounce,
    float damping,
    float friction_coeff,
    double delta
  );
}