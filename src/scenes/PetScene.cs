using System.Numerics;
using digipet.component;
using digipet.framework;
using digipet.image;
using digipet.input;
using digipet.sim;
using digipet.sprite;
using digipet.sprite.attrib;
using digipet.transition;
using digipet.transition.animator;
using digipet.transition.state;
using digipet.util;
using digipet.view;
using digipet.view.pet;
using digipet.view.world;
using digipet.world;
using digipet.world.fixture;
using digipet.world.pet;
using digipet.world.pet.animation.handlers;
using digipet.world.pet.task.edible;
using digipet.world.pet.task.tasks;
using digipet.world.pet.task.water;

namespace digipet.scenes;

#nullable enable

public class PetScene : Scene {

  private readonly CompoundView root;
  private readonly ISpriteFetcher fetcher;
  private readonly IPhysWorld physWorld;

  private readonly ILogger logger;

  private readonly SimProvider provider;

  public PetScene(IEngine engine, ISpriteFetcher fetcher) : base(engine) {
    root = new();
    this.fetcher = fetcher;
    physWorld = engine.GetPhysWorld();
    logger = this.GetLogger();

    provider = new();
  }

  public override bool HandleInput(InputType type, InputState state) {
    if (type == InputType.LEFT && state == InputState.PRESS) {
      CreateQuickMenu();
      return true;
    } else if (type == InputType.RIGHT && state == InputState.PRESS) {
      CreateStatList();
      return true;
    }
    
    return false;
  }

  private void CreateQuickMenu() {
    QuickMenu menu = new(
      Engine, provider
    ) {
      SizeX = 0.38f,
      SizeY = 1.0f,
      Offset = new(0.0f, 0.0f),
    };

    TransitionStateBuilder bb = new();
    bb.Animate(menu, "X", -0.38f, 0.0f, EasingFunctions.EaseOutQuart)
      .WithDuration(0.4f);
    menu.EnqueueTransition(bb.Build());
    
    PushToStack(menu);
  }

  private void CreateStatList() {
    PetStatList list = new(Engine.GetSpriteFetcher(), provider.GetPetModel()) {
      SizeX = 0.38f,
      SizeY = 1.0f,
      Anchor = new(1.0f, 0.0f),
      Offset = new(1.0f, 0.0f)
    };

    TransitionStateBuilder bb = new();
    bb.Animate(list, "X", 1.38f, 1.0f, EasingFunctions.EaseOutQuart)
      .WithDuration(0.4f);
    list.EnqueueTransition(bb.Build());

    PushToStack(list);
  }

  public override void InitScene() {
    PushToStack(root);
    
    IAnimatedSprite s = fetcher.GetAnimatedSprite(SpriteID.SPRITE_PET);
    s.HFrames = 2;
    s.VFrames = 1;

    
    SimplePetSprite pet = new(Engine);
    HandlePetBehavior(pet.GetController());

    root.AddView(pet);
    root.AddView(new ObjectShowView(physWorld) {
      Offset = Vector2.Zero,
      Size = Vector2.One
    });

    EnqueueTransition(GetTransition());

    logger.Log("initialized pet scene!");
  }

  public override void Tick(double delta) {
    // do nothing
  }

  private ITransition GetTransition() {
    TransitionBuilder b = new();
    SlideDownToScene(b);
    return b.Build();
  }

  private void SlideDownToScene(TransitionBuilder b) {
    // does it make sense to give scenes the ability to run a transition by default?
    TransitionStateBuilder bb = new();
    bb.Animate(root, "Y", 1.0f, 0.0f, EasingFunctions.EaseOutQuart).WithDuration(1.0f);
    // mark transitions as "blocking" to deny inputs until complete??
    b.AddState(bb.Build());

  }



  private void HandlePetBehavior(IPetController controller) {

    IPhysObject bowl = physWorld.SpawnObject(
      new SimpleWaterFixture(provider.GetWaterSource()),
      new Vector2(-0.2f, 0.0f),
      Vector2.Zero
    );

    controller.AddAnimationHandler(
      PetAnimation.IDLE, 
      new IdleHandler(Engine)
    );

    controller.AddAnimationHandler(
      PetAnimation.PAUSE,
      new PauseHandler(Engine)
    );

    controller.AddAnimationHandler(
      PetAnimation.EXPRESS,
      new IdleHandler(Engine, 1.0, PetAnimation.EXPRESS)
    );

    controller.AddAnimationHandler(
      PetAnimation.ACTIVE,
      new IdleHandler(Engine, 0.25, PetAnimation.ACTIVE)
    );

    controller.AddAnimationHandler(
      PetAnimation.REJECT,
      new RejectHandler(Engine)
    );

    controller.AddTask(new PassiveTask());
    controller.AddTask(new MunchTask(Engine, provider));
    controller.AddTask(new QuaffTask(provider.GetPetModel(), bowl, provider.GetWaterSource()));

    // task for drinking water
    // p much same as the munch task but with different steps 
  }
}