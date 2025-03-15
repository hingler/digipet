using System.Numerics;
using digipet.framework;
using digipet.image;
using digipet.rpg;
using digipet.sim;
using digipet.world.simple;

namespace digipet.world.scroll;

public class SideScrollerWorld : IWorldManager, IPhysWorld {

  private DampedObjectShow objectShow;
  private readonly WallBoundaryHandler border_L;
  private readonly WallBoundaryHandler border_R;

  public float MinX {
    get => border_L.WorldOrigin.X;
    set => border_L.WorldOrigin = new(value, 0.0f);
  }

  public float MaxX {
    get => border_R.WorldOrigin.X;
    set => border_R.WorldOrigin = new(value, 0.0f);
  }

    public float FloorHeight => objectShow.FloorHeight;

    public Vector2 Gravity { get => objectShow.Gravity; set => objectShow.Gravity = value; }

    // char controller - either a method of commanding around a character or a method of controlling it directly
    public SideScrollerWorld(IEngine engine) {
    objectShow = new(engine);
    
    WallBoundaryHandler floor = new() {
      WorldOrigin = Vector2.Zero
    };

    border_L = new() {
      WorldOrigin = new(-15.0f, 0.0f),
      WorldNormal = Vector2.UnitX
    };

    border_R = new() {
      WorldOrigin = new(15.0f, 0.0f),
      WorldNormal = -Vector2.UnitX
    };

    objectShow.AddBoundaryHandler(floor);
    objectShow.AddBoundaryHandler(border_L);
    objectShow.AddBoundaryHandler(border_R);
  }

  public void Update(double delta) => objectShow.Update((float)delta);

  public IPhysObject CreateObject(
    Vector2 spawn_pos,
    ISprite sprite
  ) {
    EmptyWorldItem item = new(sprite);
    // this is fine for now i think

    // reuse grid to verify that this works

    return objectShow.SpawnObject(item, spawn_pos, Vector2.Zero);
  }

  public IEnumerable<IWorldEntity> GetEntities() => objectShow.GetPhysObjects();
  public IEnumerable<IPhysObject> GetPhysObjects() => objectShow.GetPhysObjects();
  public void RemovePhysObject(IPhysObject obj) => objectShow.RemovePhysObject(obj);

    public IPhysObject SpawnObject(IWorldItem pickup) => 
      objectShow.SpawnObject(pickup);

    public IPhysObject SpawnObject(IWorldItem pickup, Vector2 position) => 
      objectShow.SpawnObject(pickup, position);

    public IPhysObject SpawnObject(
      IWorldItem pickup, 
      Vector2 position, 
      Vector2 velocity
    ) => 
      objectShow.SpawnObject(pickup, position, velocity);

    public IPhysObject SpawnObject(
      IWorldItem pickup, 
      Vector2 position, 
      Vector2 velocity, 
      float bounciness, 
      float linearDamping, 
      float frictionDamping
    ) => 
      objectShow.SpawnObject(pickup, position, velocity, bounciness, linearDamping, frictionDamping);

    public IReadOnlyList<CollisionData> GetCollisions(IPhysObject ob) => objectShow.GetCollisions(ob);
    // player controller is a phys object
}