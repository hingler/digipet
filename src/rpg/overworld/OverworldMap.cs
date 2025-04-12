using digipet.view.rpg;
using graphui.graph;

namespace digipet.rpg.overworld;

public struct OverworldMap {
  public string world_name;
  // nodes built

  // tba: reset?
  public IDataMappedGraphNavigator<OverworldNode> graph_nav;
  public List<IRPGDisplay> backgrounds;
}