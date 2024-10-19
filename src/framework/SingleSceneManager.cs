using digipet.component;

namespace digipet.framework;

public class SingleSceneManager {
  private readonly Scene scene;
  private readonly ICanvas canvas;
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
}