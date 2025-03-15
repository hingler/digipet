using digipet.image;
using digipet.rpg;

namespace digipet.world.scroll;

public class BasicWorld : IWorldManager {

  // treat this as a "world base"
  // 
  private readonly HashSet<IWorldEntity> entities = [];

  public BasicWorld() {}

  public void CreateWorldEntity(IWorldEntity entity) {
    entities.Add(entity);
  }

  // atp: we're doing nothing!! what's the point?
  // what *should* we be doing?

  // - shortcut for creating static entities
  // - what about making our character fall to the ground?
  //   - wrap behavior - force user to pass a "character instance" in
  public void Tick(double delta) {
    foreach (IWorldEntity entity in entities) {
      entity.Tick(delta);
    }
  }

  public IEnumerable<IWorldEntity> GetEntities() => entities;
}

// create a char controller world

// char moves around
// return char so we can center camera