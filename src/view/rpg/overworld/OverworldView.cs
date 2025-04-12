// view for a single overworld

using System.Numerics;
using digipet.component;
using digipet.framework;
using digipet.rpg.overworld;
using digipet.util;
using digipet.view.rpg;
using digipet.view.world;
using graphui.graph;
using graphui.node;

#nullable enable

public class OverworldView : ViewComponent {
  private readonly OverworldManager overworld;
  private readonly RPGDelegateView world_view;
  private readonly Vec2Lerper lerper;
  private static readonly ILogger logger = LoggerSingleton.GetStaticLogger<OverworldView>();
  public OverworldView(
    OverworldMap map,
    IEngine engine
  ) {
    overworld = new(map.graph_nav, engine);
    world_view = new(engine);
    lerper = new(10.0f);

    world_view.WorldScale = 0.1f;

    lerper.Target = overworld.GetSelectorPosition();
    lerper.Reset();

    AddView(world_view);

    foreach (IRPGDisplay display in map.backgrounds) {
      world_view.AddDisplay(display);
    }

    world_view.AddDisplay(
      BuildLineView(map)
    );

    world_view.AddDisplay(new WorldEntityWrap(overworld));
    overworld.SetSpriteScale(Vector2.One * 0.1f);
    
    world_view.Size = Vector2.One;
  }

  // receive input from UI graph
  public bool OnInput(Direction dir) {
    if (dir != Direction.NONE) {
      return overworld.ProvideInput(dir);
    }

    return false;
  }

  public override void Tick(double delta) {
    overworld.Tick(delta);

    lerper.Target = overworld.GetSelectorPosition();
    lerper.Tick(delta);

    world_view.WorldOrigin = lerper.Cursor;
  }

  public static LineView BuildLineView(
    OverworldMap map
  ) {
    IDataMappedGraphNavigator<OverworldNode> nodes = map.graph_nav;
    INodeGraph graph = nodes.GetNodeGraph();

    HashSet<OverworldNode> node_set = [];
    LineView view = new();

    IGraphNode? init_node = graph.GetInitialNode();
    if (init_node != null) {
      BuildLineView_recurse(
        init_node,
        nodes,
        node_set,
        view
      );
    }

    return view;
  }

  public static void BuildLineView_recurse(
    IGraphNode current_node,
    IDataMappedGraphNavigator<OverworldNode> node_map,
    HashSet<OverworldNode> nodes_visited,
    LineView line_view
  ) {
    OverworldNode? origin = node_map.GetDataAtNode(current_node);

    if (origin == null) {
      return;
    }

    logger.Log("loop");
    foreach (Direction dir in Enum.GetValues<Direction>()) {
      IGraphNode? dest_node = current_node.Follow(dir);
      if (dest_node == null) {
        continue;
      }

      OverworldNode? next_node = node_map.GetDataAtNode(dest_node);

      if (next_node != null && !nodes_visited.Contains(next_node)) {

        line_view.AddLine(
          origin.Position, next_node.Position, DigiColor.BLACK, 4.0f
        );

        line_view.AddLine(
          origin.Position, next_node.Position, DigiColor.WHITE, 2.0f
        );

        // line_view.AddLine(
        //   origin.Position, next_node.Position, DigiColor.BLACK, 1.0f
        // );


        nodes_visited.Add(next_node);
        BuildLineView_recurse(
          dest_node,
          node_map,
          nodes_visited,
          line_view
        );
      }
    }
  }
}