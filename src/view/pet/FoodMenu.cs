using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using digipet.component;
using digipet.framework;
using digipet.input;
using digipet.sim.db;
using digipet.sim.edible;
using digipet.user;
using digipet.user.inventory;
using digipet.util;
using digipet.util.text;
using digipet.view.container;
using digipet.view.menu;
using digipet.world;

namespace digipet.view.pet;

#nullable enable

public class FoodMenu : ViewComponent {
  private readonly BorderContainer container = new();
  private readonly ComponentMenu menu;
  private readonly ILogger logger = LoggerSingleton.GetLogger();
  private readonly HashSet<Action> confirm_actions = new();
  private readonly ISimRepo<IEdiblePickup>? edibles;
  private readonly IUserData userData;
  private readonly IPhysWorld physWorld;
  public override IReadOnlyList<ViewComponent> GetChildren() {
    return [ container ];
  }

  public FoodMenu(IEngine engine, IUserData data) {
    menu = new(engine);
    userData = data;
    // fetch db here
    // build in object world
    // just make sure it spawns!

    // (tba: if i need it, make a "complex" dims class which combines rel/abs together!!!)
    // (ex: 100% - 16px)

    edibles = engine.GetAssetRepo<IEdiblePickup>();
    
    edibles?.Let(db => {
      foreach (IEdiblePickup pickup in db.GetEntries()) {
        IInventoryItem inventory = userData.GetInventory().GetItem(pickup.RID);

        if (inventory.Quantity > 0) {
          InventoryView item_view = new(canvas.font.FontType.TINY) {
            Name = pickup.Name.Truncate(18),
            Datum = "x" + inventory.Quantity.ToString(),
            PixelSizeY = 11.0f,
            TextColor = DigiColor.BLACK
          };

          menu.AddItem(item_view, (int _) => HandleFoodPickup(pickup.RID));
        }
      }
    });

    container.AddView(menu);

    physWorld = engine.GetPhysWorld();
  }

  public void HandleFoodPickup(int RID) {
    edibles?.Let(db => {
      IEdiblePickup selection = db.Fetch(RID);
      // connect to physworld logic :3
      if (selection != null) {
        bool removed = userData.GetInventory().RemoveFromInventory(RID);
        if (removed) {
          physWorld.SpawnObject(selection);
          logger.Log("selected: ", selection.Name);
        } else {
          logger.Error("selected item which is not avail in inventory!!");
        }
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