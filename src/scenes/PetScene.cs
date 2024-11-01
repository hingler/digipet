using digipet.component;
using digipet.framework;
using digipet.image;
using digipet.input;
using digipet.sprite;
using digipet.sprite.attrib;
using digipet.transition;
using digipet.transition.animator;
using digipet.transition.state;
using digipet.util;
using digipet.view;

namespace digipet.scenes;

#nullable enable

public class PetScene : Scene {

  private readonly CompoundView root;
  private readonly ISpriteFetcher fetcher;

  private readonly ILogger logger = LoggerSingleton.GetLogger();

  public PetScene(IEngine engine, ISpriteFetcher fetcher) : base(engine) {
    root = new();
    this.fetcher = fetcher;
  }

  public override bool HandleInput(InputType type, InputState state) {
    return false;
  }

  public override void InitScene() {
    PushToStack(root);

    IAnimatedSprite s = fetcher.GetAnimatedSprite(SpriteID.SPRITE_PET);
    s.HFrames = 2;
    s.VFrames = 1;

    SimplePetSprite pet = new(s);
    root.AddView(pet);

    EnqueueTransition(GetTransition());

    logger.Log("initialized pet scene!");
  }

  public override void Tick(double delta) {
  }

  public ITransition GetTransition() {
    TransitionBuilder b = new();
    SlideDownToScene(b);
    return b.Build();
  }

  public void SlideDownToScene(TransitionBuilder b) {
    // does it make sense to give scenes the ability to run a transition by default?
    TransitionStateBuilder bb = new();
    bb.Animate(root, "Y", 1.0f, 0.0f, EasingFunctions.EaseOutQuart).WithDuration(1.0f);
    // mark transitions as "blocking" to deny inputs until complete??
    b.AddState(bb.Build());

  }
}