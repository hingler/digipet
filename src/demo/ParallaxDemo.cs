using digipet.component;
using digipet.framework;
using digipet.view.gpr;
using digipet.view.rpg;
using digipet.view.world;

public class ParallaxDemo : ViewComponent {

  private readonly RPGDelegateView view;
  private double delta_acc = 0.0;
  private double dx = 0.0;
  public ParallaxDemo(IEngine engine, ParallaxBGBuilder builder) {
    view = builder.Build(engine);
    AddView(view);

    view.WorldScale = 0.1f;
    view.Size = new(1.0f);

    view.AddDisplay(new RPGGrid());
  }

  public override void Tick(double delta) {
    dx += delta;
    delta_acc += (Math.Sin(dx * 6.0) + 1.5f) * delta * 4.5;
    view.WorldOrigin = new((float)delta_acc, 4.0f);
  }
}