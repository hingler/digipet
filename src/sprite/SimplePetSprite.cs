using System.Numerics;
using digipet.component;
using digipet.framework;
using digipet.image;
using digipet.pet;
using digipet.world.pet;
using digipet.world.pet.animation.handlers;
using digipet.world.pet.controller;
using digipet.world.pet.task.edible;
using digipet.world.pet.task.tasks;

namespace digipet.sprite;

public class SimplePetSprite : ViewComponent {
  private readonly SimplePetController controller;
  private readonly IEngine engine;

  public float Scale = 2.0f;

  // thinking: maintain a pixel scale??
  // (ex. 2 sprite-px per screenpx)

  public SimplePetSprite(IEngine engine) {
    controller = new(engine);
    this.engine = engine;
  }

  public IPetController GetController() {
    return controller;
  }

  public override void Tick(double delta) {
    base.Tick(delta);
    controller.Update(delta);
  }

  public override void Draw(ICanvas canvas) {
    base.Draw(canvas);

    // sprite centered in rect with 2 sprite-px per screen-px
    // thinking:
    // - we have an emote layer, and a pet layer
    IPetStateReadOnly state = controller.GetPetState();
    ISprite sprite = state.Animation?.Sprite;

    PetEmote emote = state.Emote;
    ISprite emote_sprite = engine.GetSpriteFetcher().GetSprite(attrib.SpriteID.OFFSET_EMOTE + (int)emote);

    Vector2 world_pos = state.Position;
    Vector2 canvas_pos = new(0.5f + world_pos.X, 0.8f - world_pos.Y);

    Vector2 pixel_size = canvas.GetPixelDims();
    Vector2 screen_size = pixel_size * Scale * (sprite?.Dims ?? Vector2.One);
    Vector2 screen_start = new(canvas_pos.X - (screen_size.X / 2.0f), canvas_pos.Y - screen_size.Y);

    Vector2 emote_start = screen_start + ((state.Animation?.FaceOffsetPx ?? Vector2.Zero) * Scale * pixel_size);
    


    if (state.Facing.X >= 0.0f) {
      canvas.Tex(sprite, screen_start, screen_start + screen_size, false);
      canvas.Tex(emote_sprite, emote_start, emote_start + screen_size, false);
    } else {
      Vector2 screen_size_x = new(screen_size.X, 0.0f);
      Vector2 screen_size_y = new(0.0f, screen_size.Y);
      canvas.Tex(sprite, screen_start, screen_start + screen_size_y - screen_size_x, false);
      canvas.Tex(emote_sprite, emote_start, emote_start + screen_size_y - screen_size_x, false);
    }
  }
}