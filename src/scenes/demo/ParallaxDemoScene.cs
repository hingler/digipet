using System.Numerics;
using digipet.component;
using digipet.file;
using digipet.framework;
using digipet.image;
using digipet.util;
using digipet.view.world;

namespace digipet.scenes.demo;

public class ParallaxDemoScene(IEngine engine) : Scene(engine) {
  public override void InitScene() {
    ParallaxBGBuilder builder = new();
    IFileLoader loader = Engine.GetDigipetAssetLoader();

    ISprite fence = loader.LoadSprite("sprites/rpg/bg/fence.png");
    ISprite grass = loader.LoadSprite("sprites/rpg/bg/grass.png");
    ISprite path = loader.LoadSprite("sprites/rpg/bg/path_stencil.png");
    ISprite water = loader.LoadSprite("sprites/rpg/bg/water01.png");
    ISprite mountain = loader.LoadSprite("sprites/rpg/bg/mountain.png");


    ILogger logger = this.GetLogger();
    logger.Log(fence);
    logger.Log(grass);

    builder.AddDynamic(
      fence, new Vector2(0.0f, 2.0f), 0.1f, 1.25f, true
    );

    builder.AddDynamic(
      grass, new Vector2(0.0f, 1.8f), 0.1f, 1.3f, true
    );

    builder.AddDynamic(
      path, new(0.0f, 0.0f), 0.1f, 1.01f, true
    );

    builder.AddDynamic(
      water, new(0.0f, 2.0f), 0.1f, 2.5f, true
    );

    builder.AddDynamic(
      mountain, new(0.0f, 3.6f), 0.1f, 5.0f, true
    );

    builder.AddSolid(
      new DigiColor(0.64f, 0.72f, 0.8f, 1.0f), 100.0f
    );
    
    ParallaxDemo demo = new(Engine, builder);

    PushToStack(demo);
  }
}