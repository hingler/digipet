using System.Collections.Generic;
using System.Numerics;
using digipet.component;
using digipet.framework;
using digipet.world;

namespace digipet.view.world;

public class ObjectShowView : ViewComponent {
  private static readonly Vector2 VIEW_OFFSET = new(0.5f, 0.0f);
  private readonly CompoundView root = new();
  private readonly Dictionary<IPhysObject, SpriteView> sprites;
  private readonly IObjectShow objectShow;

  public ObjectShowView(IObjectShow objectShow) : base() {
    sprites = [];
    this.objectShow = objectShow;
  }

  // for each phys object
  // - create a unique sprite tracking it

  public override void Draw(ICanvas canvas) {
    ICollection<IPhysObject> objects = objectShow.GetPhysObjects();

    foreach (IPhysObject o in objects) {
      if (!sprites.TryGetValue(o, out SpriteView sprite)) {
        sprite = new(o.Sprite) {
          Anchor = new(0.5f, 1.0f)
        };

        root.AddView(sprite);
        sprites[o] = sprite;
      }

      sprite.Offset = o.Position + VIEW_OFFSET;
      sprite.Size = canvas.PxToRelative(sprite.Dims);
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

    root.PreDraw(canvas);
  }
}