using System;
using System.Collections.Generic;
using digipet.canvas;
using digipet.canvas.font;
using digipet.component;
using digipet.db;
using digipet.file;
using digipet.input;
using digipet.sim;
using digipet.sim.db;
using digipet.sim.edible;
using digipet.sprite.attrib;
using digipet.util;
using digipet.world;
using digipet.world.simple;
using static digipet.util.Closure;

namespace digipet.framework.engine;

#nullable enable

public class DigiEngine : IEngine, IInputListener {
  private readonly IEngineBase platform_base;
  private readonly Stack<Scene> scene_stack = new();
  private readonly PhysicsObjectShow phys_world;
  private readonly Dictionary<Type, object> repos = [];
  private readonly BabyTimer timer = new();

  private bool debug = false;

  public DigiEngine(IEngineBase platform_base) {
    this.platform_base = platform_base;
    phys_world = new PhysicsObjectShow(this);
    platform_base.GetInputManager().Register(this);

    InitDB();
  }

  private void InitDB() {
    repos[typeof(IEdiblePickup)] = new FoodRepo(this);

    // (tba: handle finished scenes - thinking we can just crawl up and remove finished scenes)
  }

  private Scene? GetActiveScene() {
    scene_stack.TryPeek(out Scene? result);
    return result;
  }

  public void PushScene(Scene scene) {
    GetActiveScene()?.Deactivate();
    // need to be able to popscene
    // alt: if top scene is ever finished after a tick, pop it
    scene_stack.Push(scene);
  }

  public ISimRepo<T>? GetAssetRepo<T>() where T : IWorldItem {
    Type dataType = typeof(T);
    if (repos.TryGetValue(dataType, out object? value)) {
      return value as ISimRepo<T>;
    }

    return null;
  }

  public IPhysWorld GetPhysWorld() {
    return phys_world;
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

  public IFileLoader GetResourceLoader() {
    return platform_base.GetResourceLoader();
  }

  public IFileLoader GetUserdataLoader() {
    return platform_base.GetUserdataLoader();
  }

  public IFileLoader GetDigipetAssetLoader() {
    return platform_base.GetDigipetAssetLoader();
  }

  public IDataStore GetSaveStore() {
    return platform_base.GetSaveStore();
  }

  public IFontHelper GetFontHelper() {
    return platform_base.GetFontHelper();
  }

  public void TearDown() {
    // tear down
    GetActiveScene()?.PreDestroy();
    platform_base.TearDown();
  }

  public void OnInput(InputType type, InputState state) {
    // GetActiveScene()?.PreInput(type, state);
    // deprecate in favor of onkey
  }

  public void OnKey(IKeyEvent ev) {
    GetActiveScene()?.PreInput(ev);
  }

  public void Update(double delta) {
    GetActiveScene()?.Let(s => {
      if (!s.Initialized()) {
        s.Create();
        s.PreActivate();
      }
    });

    // update phys world here

    timer.Start("update");
    phys_world.Update(delta);
    GetActiveScene()?.SceneTick(delta);
    timer.End("update", debug);
  }

  public void Draw(ICanvas canvas) {
    timer.Start("draw");
    timer.Start("draw_scene");
    GetActiveScene()?.Draw(canvas);
    timer.End("draw_scene", debug);
    timer.Start("draw_flush");
    canvas.Flush();
    timer.End("draw_flush", debug);
    timer.End("draw", debug);
  }
}