using digipet.rpg;

namespace digipet.world;

public interface IWorldManager {
  IEnumerable<IWorldEntity> GetEntities();
}