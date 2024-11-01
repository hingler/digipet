using System;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Numerics;
using digipet.canvas.font;
using digipet.component;
using digipet.framework;
using digipet.sprite.attrib;
using digipet.transition;
using digipet.transition.animator;
using digipet.transition.state;
using digipet.view.sky;

namespace digipet.scenes;

public class ReviewScene : Scene {
  private readonly ISpriteFetcher fetcher;
  private readonly StarrySky stars;
  private readonly SkyView sky;
  private readonly StepCounterWrap counter;

  private readonly SkyCounterMini steps;
  private readonly SkyCounterMini sleep;

  private readonly IEngine engine;

  public double step_count = 0.0;
  public ReviewScene(IEngine engine, ISpriteFetcher fetcher) : base(engine) {
    this.engine = engine;
    this.fetcher = fetcher;
    sky = new(fetcher);
    stars = new(7);

    counter = new(
      fetcher
    ) {
      StepCount = 0,
      Anchor = new(0.5f, 0.0f),
      Size = new(1.0f, 0.6f),
      Offset = new(0.5f, 0.2f)
    };

    steps = new(fetcher.GetSprite(SpriteID.ICON_STEP_12PX)) {
      Content = "0",
      Size = new(0.8f, 0.08f),
      Offset = new(0.1f, 0.1f),
      Opacity = 0.0f
    };

    sleep = new(fetcher.GetSprite(SpriteID.ICON_SLEEP_12PX)) {
      Content = "0",
      Size = new(0.8f, 0.08f),
      Offset = new(0.1f, 0.08f),
      Opacity = 0.0f
    };
  }

  public override void InitScene() {
    PushToStack(stars);
    PushToStack(sky);
    

    // show a little text modal if we go above our max

    PushToStack(counter);

    EnqueueTransition(GetTransition());

    PushToStack(steps);
    PushToStack(sleep);
  }

  public override void Tick(double delta) {
    base.Tick(delta);
  }

  public ITransition GetTransition() {
    TransitionBuilder bb = new();
    PanDownToSky(bb);
    ThenCountSteps(bb);
    ThenFlickUpSteps(bb);
    TransitionOutIntoClouds(bb);
    ThenPushPetScene(bb);

    return bb.Build();
  }

  // fetch transitions
  public void PanDownToSky(TransitionBuilder builder) {
    TransitionStateBuilder stateBuilder = new();
    stateBuilder
      .Animate(sky, "DepthOffset", -250.0, 0.0, EasingFunctions.EaseOutQuart)
      .Animate(stars, "fade_start", 1.2f, 0.75f, EasingFunctions.EaseOutQuart)
      .Animate(stars, "fade_end", 1.4f, 0.9f, EasingFunctions.EaseOutQuart)
      .Animate(stars, "StarOffset", 0.2f, 0.0f, EasingFunctions.EaseOutQuart)
      .Animate(counter, "Y", 0.8f, 0.2f, EasingFunctions.EaseOutQuart)
      .Animate(counter, "Opacity", 0.0f, 1.0f, EasingFunctions.EaseOutQuad)
      .WithDuration(3.0);

    

    builder.Then(stateBuilder.Build());
  }

  public void ThenCountSteps(TransitionBuilder builder) {
    builder.ThenCall(() => counter.Opacity = 1.0f);
    
    TransitionStateBuilder stateBuilder = new();

    stateBuilder
      .Animate(counter, "StepCount", 0.0, 9556.0, EasingFunctions.EaseInOutQuart)
      .WithDuration(4.5);

    builder.Then(stateBuilder.Build()); 
    builder.ThenPause(1.0);
  }

  public void ThenFlickUpSteps(TransitionBuilder builder) {
    TransitionStateBuilder stateBuilder = new();

    stateBuilder
    .Animate(counter, "Y", 0.2f, 0.1f, EasingFunctions.EaseInBackQuart)
    .WithDuration(0.38f);

    builder.Then(stateBuilder.Build());
    builder.ThenCall(() => {
      steps.Opacity = 1.0f;
      counter.Opacity = 0.0f;
      steps.Content = counter.StepCount.ToString();
    });
    
    stateBuilder = new();
    stateBuilder
      .Animate(steps, "Y", 0.06f, 0.0f, EasingFunctions.EaseOutQuint)
      .WithDuration(0.4);

    builder.Then(stateBuilder.Build());
    builder.ThenPause(1.2);
  }

  public void TransitionOutIntoClouds(TransitionBuilder builder) {
    TransitionStateBuilder stateBuilder = new();

    EasingFunction ease = EasingFunctions.EaseInQuart;
    stateBuilder
      .Animate(sky, "DepthOffset", 0.0f, 375.0, ease)
      .Animate(stars, "StarOffset", 0.0f, -0.3f, ease)
      .Animate(steps, "Y", 0.0f, -1.8f, ease)
      .WithDuration(2.0);

    builder.Then(stateBuilder.Build());
  }

  public void ThenPushPetScene(TransitionBuilder builder) {
    builder.ThenCall(() => {
      engine.PushScene(new PetScene(engine, fetcher));
      Finish();
    });
  }

  // write ease in funcs
  // write code to "flick" up step count and then pick up tx up top

  // then: do the same transition with sleep
  // then: offset up, cover screen
  // then: pass along to pet view, which will scroll up to 0!
}