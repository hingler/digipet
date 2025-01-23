using System.Collections.Generic;
using System.Numerics;
using digipet.framework;
using digipet.image;
using digipet.sim;
using digipet.sprite.attrib;
using digipet.util;
using static digipet.util.Closure;

using static digipet.util.LoggerSingleton;

namespace digipet.world.simple;

#nullable enable

public class SimpleObjectShow : IPhysWorld {
  
  private IPhysObject? obj;
  private readonly ISpriteFetcher fetcher;
  private readonly ILogger log;

  public float FloorHeight { get => 0.8f; }
  
  public SimpleObjectShow(IEngine engine) {
    fetcher = engine.GetSpriteFetcher();
    obj = null;

    log = this.GetLogger();
  }

  public IPhysObject SpawnObject(
    IWorldItem pickup,
    Vector2 position,
    Vector2 velocity,
    float bounciness,
    float linearDamping,
    float frictionDamping
  ) => SpawnObject(pickup);

  public IPhysObject SpawnObject(IWorldItem pickup, Vector2 position, Vector2 velocity) => SpawnObject(pickup);
  public IPhysObject SpawnObject(IWorldItem pickup, Vector2 position) => SpawnObject(pickup);

  public IPhysObject SpawnObject(
    IWorldItem pickup
  ) {
    if (obj != null) {
      return obj;
    }

    // treat objects as point parti cles for now
    obj = new SimplePhysObject(new Vector2(0.2f, 0.0f), fetcher.GetSprite(pickup.RID), pickup);
    log.Log("created new object: ", obj.Sprite);
    return obj;
  }

  public IEnumerable<IPhysObject> GetPhysObjects() {
    HashSet<IPhysObject> res = [];
    if (obj != null) {
      obj?.Let((o) => res.Add(o));
    }

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