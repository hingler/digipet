using digipet.component;
using digipet.framework;
using digipet.input;
using digipet.sprite.attrib;
using digipet.view.menu.carousel;

namespace digipet.scenes.demo;

public class CarouselMenuDemo : Scene {
  private readonly CarouselMenu menu;
  public CarouselMenuDemo(IEngine engine) : base(engine) {
    menu = new(Engine);
  }


  public override void InitScene() {
    for (int i = 0; i < 10; i++) {
      menu.AddItem(
        new StubMenuItem("hello!" + i) {
          Sprite = Engine.GetSpriteFetcher().GetSprite(((i % 4) == 3) ? SpriteID.NPC_OWL : (SpriteID.OFFSET_FOOD + (i % 4)))
        }
      );
    }

    PushToStack(menu);
  }

  public override bool HandleInput(InputType type, InputState state) {
    base.HandleInput(type, state);

    if (state == InputState.PRESS) {
      if (type == InputType.LEFT) {
        menu.Target--;
      } else if (type == InputType.RIGHT) {
        menu.Target++;
      }
    }

    return true;
  }
}