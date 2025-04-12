using digipet.rpg;
using digipet.sim;
using graphui.builder;
using graphui.graph;
using graphui.node;

namespace digipet.scenes.rpg.charselect;

#nullable enable

public class CharSelectCoordinator {
  private readonly InputReceivingGraphNavigator<ICharSelectModel> ui_graph;
  private readonly PartyViewSelectModel party_model;

  private int SrcIndex = -1;
  private ICharData? SrcData = null;

  public CharSelectCoordinator(
    PartyViewSelectModel party_model,
    CharPickerSelectModel picker_model
  ) {
    SimpleGraphBuilder<ICharSelectModel> builder = new();
    builder.Connect(party_model, picker_model, Direction.DOWN);
    // throw err if this fails
    IDataMappedGraphNavigator<ICharSelectModel> model = builder.Build()!;


    ui_graph = new InputReceivingGraphNavigator<ICharSelectModel>(model);
    this.party_model = party_model;
  }

  public bool Step(Direction dir) => ui_graph.Step(dir);

  // two phases of select: first click, and second click
  public void Select() {
    if (SrcData != null || SrcIndex >= 0) {
      // we've selected something prior - second click
      ICharData dst_data = party_model.SwapIn(SrcData, party_model.GetSelectedIndex());
      if (SrcIndex >= 0) {
        // swap dst data back to src, if the src index is valid
        party_model.SwapIn(dst_data, SrcIndex);
      }

      SrcIndex = -1;
      SrcData = null;
      party_model.Lock = false;
    } else {
      SrcData = ui_graph.GetActiveData()?.GetSelectedCharData() ?? null;
      SrcIndex = ui_graph.GetActiveData()?.GetSelectedIndex() ?? -1;
      party_model.Lock = true;
      // step into party model and lock st we can't get out until we select
      ui_graph.ForceStep(Direction.UP);
    }
  }
}