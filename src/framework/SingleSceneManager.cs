using digipet.canvas;
using digipet.component;
using digipet.file;
using digipet.input;
using digipet.sim;
using digipet.sim.db;
using digipet.sprite.attrib;
using digipet.world;

namespace digipet.framework;

public class SingleSceneManager : IEngine {
  private readonly Scene scene;
  private readonly ICanvas canvas;
  private readonly IEngineBase base_engine;

  public SingleSceneManager(IEngineBase b) => base_engine = b;

  public void PushScene(Scene scene) { /* no op */ }
  public SingleSceneManager(Scene initScene, ICanvas canvas) {
    scene = initScene;
    this.canvas = canvas;
  }

  public void Update(double delta) {
    if (!scene.Initialized()) {
      // create + activate
      scene.Create();
      scene.PreActivate();
    }

    // tick
    scene.SceneTick(delta);
    scene.Draw(canvas);
    canvas.Flush();
  }

  public ISubCanvas CreateSubCanvas() {
    return base_engine.CreateSubCanvas();
  }

  public ISpriteFetcher GetSpriteFetcher() {
    return base_engine.GetSpriteFetcher();
  }

  public IInputManager GetInputManager() {
    return base_engine.GetInputManager();
  }

  public IFileLoader GetResourceLoader() {
    return base_engine.GetResourceLoader();
  }

  public IFileLoader GetUserdataLoader() {
    return base_engine.GetUserdataLoader();
  }

  public ISimRepo<T> GetAssetRepo<T>() where T : IWorldItem {
    return null;
  }

  public IPhysWorld GetPhysWorld() {
    return null;
  }
}