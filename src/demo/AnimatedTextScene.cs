using System;
using digipet.component;
using digipet.view;

namespace digipet.demo;

public class AnimatedTextScene : Scene {

  private readonly Text t;
  private double dt;

  public AnimatedTextScene() {
    dt = 0.0;
    t = new() {
      Content = "test_text",
      Scale = 4.0f,
      Origin = new(0.5f, 0.5f)
    };
  }
  public override void InitScene() {
    PushToStack(t);
  }

  public override void Tick(double delta) {
    dt += delta;
    t.Origin.X = (float)(Math.Sin(dt) * 0.4 + 0.5);
  }
}