using System.Collections.Generic;
using digipet.canvas;
using digipet.component;
using digipet.input;
using digipet.sprite.attrib;
using digipet.util;

using static digipet.util.Closure;

namespace digipet.framework.engine;

#nullable enable

public class DigiEngine : IEngine, IInputListener {
  private readonly IEngineBase platform_base;
  private readonly Stack<Scene> scene_stack = new();

  public DigiEngine(IEngineBase platform_base) {
    this.platform_base = platform_base;
    platform_base.GetInputManager().Register(this);
  }

  private Scene? GetActiveScene() {
    scene_stack.TryPeek(out Scene? result);
    return result;
  }

  public void PushScene(Scene scene) {
    GetActiveScene()?.Deactivate();
    scene_stack.Push(scene);
  }

  public ISubCanvas CreateSubCanvas() {
    return platform_base.CreateSubCanvas();
  }

  public ISpriteFetcher GetSpriteFetcher() {
    return platform_base.GetSpriteFetcher();
  }

  public IInputManager GetInputManager() {
    return platform_base.GetInputManager();
  }

  public void OnInput(InputType type, InputState state) {
    GetActiveScene()?.PreInput(type, state);
  }

  public void Update(double delta) {
    GetActiveScene()?.Let(s => {
      if (!s.Initialized()) {
        s.Create();
        s.PreActivate();
      }
    });

    GetActiveScene()?.SceneTick(delta);
  }

  public void Draw(ICanvas canvas) {
    GetActiveScene()?.Draw(canvas);
    canvas.Flush();
  }
}