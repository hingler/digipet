using digipet.canvas.font;
using digipet.component;
using digipet.framework;
using digipet.input;
using digipet.util;
using digipet.view;
using digipet.view.bg;

namespace digipet.scenes.demo.swap;

public class SceneSwapA : Scene {

  public SceneSwapA(IEngine engine) : base(engine) {}
  public override void InitScene() {
    Text t = new() {
      Content = "A",
      Font = FontType.MID,
      Offset = new(0.5f),
      Alignment = HorizontalAlign.CENTER
    };

    PushToStack(new ColorRect(DigiColor.BLACK));
    PushToStack(t);
  }

  public override bool HandleInput(IKeyEvent @event) {
    base.HandleInput(@event);
    if (@event.Action == InputType.CONFIRM && @event.State == InputState.PRESS) {
      Engine.PushScene(new SceneSwapB(Engine));
    }

    
    return true;
  }
}