using digipet.component;
using digipet.framework;
using digipet.input;
using digipet.rpg.picker;
using digipet.scenes.rpg.charselect;
using digipet.util;
using digipet.view.rpg.picker;

public class CharSelectScene : Scene {
  private readonly CharPickerMenu m;
  private readonly PartyView p;
  private static readonly ILogger logger = LoggerSingleton.GetStaticLogger<CharSelectScene>();

  private readonly CharSelectCoordinator coordinator;
  public CharSelectScene(IEngine engine) : base(engine) {
    m = new(Engine) {
      Size = new(1.0f, 0.6f),
      Anchor = new(0.5f, 0.0f),
      Offset = new(0.5f, 0.4f)
    };

    p = new(Engine) {
      Size = new(1.0f, 0.4f),
      Anchor = new(0.5f, 0.0f),
      Offset = new(0.5f, 0.0f)
    };

    coordinator = new(
      new PartyViewSelectModel(p), 
      new CharPickerSelectModel(m)
    );
  }
  public override void InitScene() {

    ViewComponent joiner = new();
    joiner.AddView(m);
    joiner.AddView(p);

    PushToStack(joiner);
  }

  // whats next?
  // - wrap pickers in a ui handler

  public override bool HandleInput(IKeyEvent @event) {
    if (base.HandleInput(@event)) {
      return true;
    }

    coordinator.Step(InputToUIDirection.Convert(@event));

    return false;
  }
}