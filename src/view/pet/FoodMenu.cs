using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using digipet.component;
using digipet.framework;
using digipet.input;
using digipet.sim.db;
using digipet.sim.edible;
using digipet.util;
using digipet.view.container;
using digipet.view.menu;
using digipet.world;

namespace digipet.view.pet;

#nullable enable

public class FoodMenu : ViewComponent {
  private readonly BorderContainer container = new();
  private readonly TextMenu menu;
  private readonly ILogger logger = LoggerSingleton.GetLogger();
  private readonly HashSet<Action> confirm_actions = new();
  private readonly ISimRepo<IEdiblePickup>? edibles;
  private readonly IPhysWorld physWorld;
  public override IReadOnlyList<ViewComponent> GetChildren() {
    return [ container ];
  }

  public FoodMenu(IEngine engine) {
    menu = new(engine, canvas.font.FontType.TINY);

    // fetch db here
    // build in object world
    // just make sure it spawns!

    // (tba: if i need it, make a "complex" dims class which combines rel/abs together!!!)
    // (ex: 100% - 16px)

    edibles = engine.GetAssetRepo<IEdiblePickup>();
    
    edibles?.Let(db => {
      foreach (IEdiblePickup pickup in db.GetEntries()) 
        menu.AddItem(pickup.Name, (int pick) => HandleFoodPickup(pickup.RID));
      
    });

    container.AddView(menu);

    physWorld = engine.GetPhysWorld();
  }

  public void HandleFoodPickup(int RID) {
    edibles?.Let(db => {
      IEdiblePickup selection = db.Fetch(RID);
      physWorld.SpawnObject(selection);

      // connect to physworld logic :3
      if (selection != null) {
        logger.Log("selected: ", selection.Name);
      } else {
        logger.Error("selection could not be found??");
      }
    });
  }

  public void AddConfirmListener(Action action) {
    confirm_actions.Add(action);
  }

  public override bool HandleInput(InputType input, InputState state) {
    if (base.HandleInput(input, state)) {
      return true;
    }

    if (state != InputState.RELEASE) {
      if (input == InputType.UP) {
        menu.DecrementSelector();
      } else if (input == InputType.DOWN) {
        menu.IncrementSelector();
      } else if (input == InputType.CONFIRM) {
        menu.ConfirmSelector();

        foreach (Action a in confirm_actions) {
          a();
        }

      } else if (input == InputType.BACK) {
        PopSelf();
      }
    }

    return true;
  }
}