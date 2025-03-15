using System.Numerics;
using graphui.builder;
using graphui.graph;
using graphui.node;

namespace digipet.rpg.overworld;

#nullable enable

public class OverworldBuilder {
  private readonly SimpleGraphBuilder<OverworldNode> graph;
  
  public OverworldBuilder() {
    graph = new();
  }

  // node A, outgoing direction, node B, incoming direction
  // figure out pathing later :3

  public OverworldBuilder Connect(
    OverworldNode node_a,
    Direction dir_out,
    OverworldNode node_b,
    Direction dir_in
  ) {
    graph.Connect(node_a, node_b, dir_out, false);
    graph.Connect(node_b, node_a, dir_in, false);

    return this;
  }

  public OverworldBuilder SetInitialNode(OverworldNode initial) {
    graph.SetInitialNode(initial);
    return this;
  }

  public IDataMappedGraphNavigator<OverworldNode>? Build() {
    return graph.Build();
  }
}