using System.Numerics;
using digipet.component;
using digipet.util;
using digipet.world;

namespace digipet.view.world;

public class ObjectShowView : ViewComponent {
  private static readonly Vector2 VIEW_OFFSET = new(0.5f, 0.8f);
  private readonly CompoundView root = new();
  private readonly Dictionary<IPhysObject, SpriteView> sprites;
  private readonly IPhysWorld objectShow;
  private readonly ILogger l = LoggerSingleton.GetLogger();

  public ObjectShowView(IPhysWorld objectShow) : base() {
    sprites = [];
    this.objectShow = objectShow;
    AddView(root);
    root.Offset = Vector2.Zero;
    root.Size = Vector2.One;
  }

  public override void Draw(ICanvas canvas) {
    IEnumerable<IPhysObject> objects = objectShow.GetPhysObjects();
    foreach (IPhysObject o in objects) {
      if (!sprites.TryGetValue(o, out SpriteView sprite)) {
        sprite = new(
          o.Sprite
        ) {
          Anchor = new(0.5f, 1.0f),
          SizePx = o.Sprite.Dims,
          Opacity = 1.0f
        };

        l.Log("created new sprite - ", o.Sprite, ", ", o.Pickup.RID);

        root.AddView(sprite);
        sprites[o] = sprite;
      }

      sprite.Offset = VIEW_OFFSET;
      sprite.X = VIEW_OFFSET.X + o.Position.X;
      sprite.Y = objectShow.FloorHeight - o.Position.Y;
      sprite.FlipX = o.FlipX;
    }

    foreach (IPhysObject o in sprites.Keys) {
      // ie: sprite `o` is no longer returned by phys world
      if (!objects.Contains(o)) {
        // delete sprite
        SpriteView sprite = sprites[o];
        root.RemoveView(sprite);
        sprites.Remove(o);
      }
    }
  }
}