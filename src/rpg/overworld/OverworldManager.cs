// thinking:
// - put nodes and selector on top
// - create panels below them

namespace digipet.rpg.overworld;

using System.Numerics;
using digipet.framework;
using digipet.rpg;
using digipet.util;
using digipet.view.rpg;
using digipet.world;
using graphui.graph;
using graphui.node;

public class OverworldManager : IWorldManager {
  private readonly IDataMappedGraphNavigator<OverworldNode> graph;
  private readonly List<NodeEntity> node_list;
  private readonly SelectorEntity selector;

  private static readonly ILogger logger = LoggerSingleton.GetStaticLogger<OverworldManager>();

  public OverworldManager(
    OverworldBuilder builder,
    IEngine engine
  ) : this(builder.Build(), engine) {}

  public OverworldManager(
    IDataMappedGraphNavigator<OverworldNode> graph,
    IEngine engine
  ) {
    this.graph = graph;
    node_list = [];

    foreach (OverworldNode on in graph.GetNodes()) {
      NodeEntity node = new(engine, on);
      node_list.Add(node);
    }

    logger.Log("built node list");

    selector = new(engine);
    logger.Log("selector constructed");
    selector.Target = graph.GetActiveData().Position;
    selector.Reset();
  }

  public Vector2 GetSelectorPosition() => selector.Position;

  public IEnumerable<IWorldEntity> GetEntities() {
    return [..node_list, selector];
  }

  public void Tick(double delta) {
    foreach (NodeEntity entity in node_list) {
      entity.Tick(delta);
    }

    selector.Tick(delta);
  }

  public void SetSpriteScale(Vector2 scale) {
    foreach (NodeEntity entity in node_list) {
      entity.SpriteScale = scale;
    }

    selector.SpriteScale = scale;
  }

  // returns true if the input was consumed, false if it was not
  public bool ProvideInput(Direction dir) {
    // datamapped navigator is self contained
    bool valid_step = graph.Step(dir);
    if (valid_step) {
      selector.Target = graph.GetActiveData().Position;
    }

    return valid_step;
  }
}