using digipet.component;
using digipet.image;
using digipet.pet;
using digipet.world.pet;
using digipet.world.pet.animation;

namespace digipet.view.npc;

public class SimpleTalkPortrait : ViewComponent, IPortrait {

  private bool talk_;
  public bool Talk {
    get => talk_;
    set {
      talk_ = value;
    }
  }

  // discard for now
  public PetEmote Emote { get; set; }

  private readonly IAnimatedSprite sprite;
  private readonly IAnimationData talk_anim;

  private readonly SpriteView sprite_display = new();

  public SimpleTalkPortrait(IAnimatedSprite talk_sprite) {
    AddView(sprite_display);

    sprite = talk_sprite;
    AnimationDataBuilder builder_talk = new();
    builder_talk.WithAnimatedSprite(sprite, 0.125, PetAnimation.IDLE, true);
    talk_anim = builder_talk.Build();

    sprite_display.Sprite = sprite;
  }

  public override void Tick(double delta) {
    base.Tick(delta);
    if (Talk) {
      talk_anim.Update(delta);
    } else if (talk_anim.Frame != 0) {
      talk_anim.Update(delta);
    }

    sprite_display.Sprite = talk_anim.GetCurrentSprite();
  }
}