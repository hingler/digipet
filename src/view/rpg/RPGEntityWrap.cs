using System.Numerics;
using digipet.component;
using digipet.rpg;
using digipet.util;
using digipet.util.dict;

namespace digipet.view.rpg;

// oh - split this into multiple visualizers??
public class RPGEntityWrap : ViewComponent, IRPGDisplay {
  private readonly CombatManager manager;

  // tba: want to have some sort of "global scale"
  private readonly Dictionary<ICombatEntity, SpriteView> sprites;
  private static readonly ILogger logger = LoggerSingleton.GetStaticLogger<RPGEntityWrap>();
  public Vector2 WorldOrigin { get; set; } = Vector2.Zero;
  public float WorldScale { get; set; } = 0.1f;
  public RPGEntityWrap(
    CombatManager manager
  ) {
    this.manager = manager;
    sprites = [];
  }

  public override void Tick(double delta) {
    // don't handle ticking manager

    IEnumerable<ICombatEntity> entities = manager.GetEntities();
    
    // keys which no longer map to an active entity
    IEnumerable<ICombatEntity> inactives = sprites.Keys.Except(entities);

    // entities which have not been mapped to a sprite view
    IEnumerable<ICombatEntity> new_elems = entities.Except(sprites.Keys);

    sprites.RemoveAll(inactives, RemoveView);
    sprites.AddAll(new_elems, CreateNewSprite);

    foreach (KeyValuePair<ICombatEntity, SpriteView> kv in sprites) {
      UpdateSprite(kv);
    }
  }

  private void UpdateSprite(KeyValuePair<ICombatEntity, SpriteView> sprite_map) {
    ICombatEntity k = sprite_map.Key;
    SpriteView v = sprite_map.Value;

    Vector2 origin_px = SizePx / 2;

    Vector2 sprite_size_px = k.WorldDims / WorldScale;
    Vector2 sprite_position_px = ((k.Position - WorldOrigin) / WorldScale) + origin_px;

    // maps 192 -> 0, 0 -> 192
    Vector2 sprite_offset_px = new(
      sprite_position_px.X, 
      SizePx.Y - sprite_position_px.Y
    );

    v.OffsetPx = sprite_offset_px;
    v.SizePx = sprite_size_px;
  }

  private SpriteView CreateNewSprite(ICombatEntity entity) {
    SpriteView sprite = new() {
      Sprite = entity.Sprite,
      Anchor = new(0.5f)
    };

    logger.Log("created sprite view at location: ", entity.Position);
    AddView(sprite);
    return sprite;
  }
}