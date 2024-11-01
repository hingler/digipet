using System.Collections.Generic;
using System.Numerics;
using digipet.image;
using digipet.sim;
using static digipet.util.Closure;

namespace digipet.world.simple;

#nullable enable

public class SimpleObjectShow : IObjectShow {
  
  private IPhysObject? obj;
  
  public SimpleObjectShow() {}

  public bool SpawnObject(
    IWorldItem pickup
  ) {
    if (obj != null) {
      return false;
    }

    // treat objects as point parti cles for now
    obj = new SimplePhysObject(Vector2.Zero, pickup);
    return true;
  }

  public ICollection<IPhysObject> GetPhysObjects() {
    ICollection<IPhysObject> res = new HashSet<IPhysObject>();
    obj?.Let(res.Add);

    return res;
  }

  public IPhysObject? FetchByPriority() {
    return obj;
  }

  public void RemovePhysObject(IPhysObject obj) {
    if (this.obj == obj) {
      this.obj = null;
    }
  }
}