using System.Numerics;
using digipet.component;
using digipet.framework;
using digipet.image;
using digipet.transition.animator;
using digipet.view;
using digipet.world.pet;

namespace digipet.diary.handlers.save;

public class DiaryStarTask : IPetTask {
  public bool Interruptable => false;

  private SpriteView star_sprite;
  private Vector2 sprite_origin;
  private double dt;

  private bool saving = false;

  private const double ANIMATION_DURATION = 0.3;
  private const float ANIMATION_DY = 0.1f;
  public DiaryStarTask(IEngine engine, Vector2 sprite_origin, ViewComponent root) {
    ISprite tex = engine.GetDigipetAssetLoader().LoadSprite("sprites/pet/star.png");
    star_sprite = new() {
      Sprite = tex,
      Opacity = 0.0f
    };

    this.sprite_origin = sprite_origin;

    star_sprite.Offset = this.sprite_origin;
    dt = 0.0;

    root.AddView(star_sprite);
  }

  // ignore save wait completely

  public void BeginTask() {
    dt = 0.0;
    star_sprite.Opacity = 1.0f;
  }

  public IPetState Update(IPetStateReadOnly prev_state, double delta) {
    dt += delta;

    double t = Math.Clamp((dt / ANIMATION_DURATION), 0, 1);

    star_sprite.Offset = sprite_origin - new Vector2(0.0f, ANIMATION_DY) * (float)EasingFunctions.EaseOutQuart(t);
    star_sprite.Opacity = (float)(1.0 - t);

    return new SimplePetObject(prev_state) {
      Emote = pet.PetEmote.Happy,
      PetState = PetAction.SLEEP
    };
  }

  // never complete - we'll exit out after save
  public bool Complete() => false;

  public int GetPriority() => saving ? 100 : -1;
}