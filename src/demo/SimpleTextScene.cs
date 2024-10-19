using digipet.component;
using digipet.view;

namespace digipet.demo;

public class SimpleTextScene : Scene {
  public SimpleTextScene() : base() {}

  // best way to enqueue?
  // tba: compound view which works like scene
  // - nop for input
  // tba2: 

  public override void InitScene() {
    Text t = new() {
      Content = "asdasd",
      Scale = 2.0f,
      Origin = new(0.5f, 0.5f)
    };

    PushToStack(t);
  }
}