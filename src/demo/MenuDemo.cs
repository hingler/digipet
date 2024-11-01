using System.Numerics;
using digipet.component;
using digipet.framework;
using digipet.input;
using digipet.util;
using digipet.view;
using digipet.view.container;
using digipet.view.menu;

namespace digipet.demo;

public class MenuDemo : Scene {
  private readonly TextMenu m;

  private readonly ILogger logger = LoggerSingleton.GetLogger();
  private readonly IEngine engine;

  public MenuDemo(
    IEngine engine
  ) : base(engine) {
    m = new(engine, canvas.font.FontType.TINY);
    this.engine = engine;
  }

  public override void InitScene() {
    BorderContainer container = new();
    container.ZIndex = 0;

    SpriteView test_sprite = new();
    test_sprite.sprite = engine.GetSpriteFetcher().GetSprite(sprite.attrib.SpriteID.STAT_FOOD);
    test_sprite.ZIndex = -100;


    PushToStack(test_sprite);
    PushToStack(container);

    // in front, but should be behind

    container.AddView(m);
    container.Size = new(0.5f, 0.8f);
    container.Offset = new(0.25f, 0.1f);
    
    m.Size = Vector2.One;
    m.Offset = Vector2.Zero;

    for (int i = 0; i < 128; i++) {
      m.AddItem(i.ToString(), (int choice) => logger.Log("selected item ", choice));
    }
  }

  public override bool HandleInput(InputType type, InputState state) {
    if (base.HandleInput(type, state)) {
      return true;
    }

    if (state != InputState.RELEASE) {
      if (type == InputType.DOWN) {
        logger.Log("increment");
        m.IncrementSelector();
      } else if (type == InputType.UP) {
        logger.Log("decrement");
        m.DecrementSelector();
      } else if (type == InputType.CONFIRM) {
        m.ConfirmSelector();
      }
    }

    return true;
  }
}