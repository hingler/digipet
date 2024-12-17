using System.Runtime.CompilerServices;
using digipet.component;
using digipet.framework;
using digipet.input;
using digipet.sim;
using digipet.sim.edible;
using digipet.transition;
using digipet.transition.animator;
using digipet.transition.state;
using digipet.user;
using digipet.util;
using digipet.view.bg;
using digipet.view.store;
using digipet.view.text;

namespace digipet.scenes;

public class StoreScene : Scene {
  private readonly StoreHandler handler;
  public StoreScene(IEngine engine, IUserData userData) : this(
    engine, 
    engine.GetAssetRepo<IEdiblePickup>().GetEntries(), 
    userData
  ) {}

  public StoreScene(
    IEngine engine,
    IReadOnlyCollection<IWorldItem> items,
    IUserData userData
  ) : base(engine) {
    handler = new StoreHandler(engine, items, userData);
  }

  public override void InitScene() {
    // nop
    PushToStack(handler);
  }

  public override bool HandleInput(IKeyEvent @event) {
    if (@event.Action == InputType.BACK && @event.State == InputState.PRESS) {
      // run logic
      LeaveStore();
    }
    
    return true;
  }

  private void LeaveStore() {
    handler.Content = "Thank you for shopping!!!";
    ColorRect rect = new(DigiColor.WHITE) {
      Opacity = 0.0f
    };

    PushToStack(rect);

    TransitionBuilder b = new();
    TransitionStateBuilder bb = new();
    bb.Animate(rect, "Opacity", 0.0f, 1.0f, EasingFunctions.EaseInOutQuad)
      .WithDuration(0.5)
      .AndBlockInput();

    b.ThenPause(3.0);
    b.Then(bb.Build());
    b.ThenPause(0.5);
    b.ThenCall(Finish);

    EnqueueTransition(b.Build());
  }
}