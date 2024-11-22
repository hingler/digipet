using System.Numerics;
using digipet.canvas.font;
using digipet.component;
using digipet.framework;
using digipet.image;
using digipet.input;
using digipet.sim;
using digipet.sprite.attrib;
using digipet.transition.text;
using digipet.user;
using digipet.util;
using digipet.view.bg;
using digipet.view.container;
using digipet.view.menu;
using digipet.view.npc;

namespace digipet.view.store;

public class StoreHandler : ViewComponent {

  private readonly MarginContainer testContainer;
  private readonly ComponentMenu menu;

  private readonly DialogueBox box;

  private readonly IList<IWorldItem> store_items = [];

  private readonly IUserData userData;

  private readonly SimpleTalkPortrait portrait;

  public StoreHandler(IEngine engine, IUserData userData) : this(engine, [], userData) {}

  public StoreHandler(
    IEngine engine,
    IReadOnlyCollection<IWorldItem> items,
    IUserData userData
  ) : base() {
    testContainer = new() {
      MarginPx = 16
    };
    this.userData = userData;

    menu = new(engine) {
      Margin = 3.0f
    };

    foreach (IWorldItem item in items) {
      InventoryView item_view = new(FontType.TINY) {
        Name = item.Name,
        Datum = item.StorePrice.ToString() + "¢",
        PixelSizeY = 11.0f,
        TextColor = DigiColor.BLACK
      };

      menu.AddItem(item_view, (int cur) => AttemptToPurchase(item));
      store_items.Add(item);


    }
    
    IAnimatedSprite portrait_sprite = engine.GetSpriteFetcher().GetAnimatedSprite(SpriteID.NPC_OWL);
    portrait_sprite.HFrames = 2;

    portrait = new SimpleTalkPortrait(portrait_sprite) {
      Anchor = new(0.5f, 1.0f),
      Offset = new(0.5f, 1.0f),
      SizePx = portrait_sprite.Dims
    };

    box = new(engine);

    CreateView(engine);
  }

  public override void Tick(double delta) {
    base.Tick(delta);
    portrait.Talk = !box.Complete;
  }

  private void CreateView(IEngine engine) {
    SpriteView bg = new(engine.GetSpriteFetcher().GetSprite(SpriteID.OFFSET_BG)) {
      Tile = true,
      Size = Vector2.One,
      Offset = Vector2.Zero
    };

    AddView(bg);

    testContainer.Size = Vector2.One;
    testContainer.Offset = Vector2.Zero;

    BevelContainer portrait_bv = new() {
      BevelSize = 1,
      Depth = -0.5f,
      SizePx = portrait.SizePx + new Vector2(4.0f, 4.0f),
      Anchor = Vector2.One,
      Offset = Vector2.One
    };
    
    portrait_bv.AddView(
      new ColorRect(DigiColor.WHITE)
    );

    // how do we want to handle expression?
    // - thinking: add another "tag type" to our strings
    // - come up with some parser spec that relays events (listener)
    //   as well as dialogue
    // - implement text flow spec and pass to box
    // - register event listener and pass emotes to portrait
    
    portrait_bv.AddView(portrait);

    testContainer.AddView(portrait_bv);

    BevelContainer menu_bv = new() {
      BevelSize = 1,
      Depth = -0.5f,
      Size = new(0.72f)
    };

    menu_bv.AddView(new ColorRect(DigiColor.WHITE));
    menu_bv.AddView(menu);

    testContainer.AddView(menu_bv);

    DialogueBox box = AddDialogueBoxToContainer(testContainer);
    box.Content = "hello!!!";

    AddView(testContainer);
  }

  private DialogueBox AddDialogueBoxToContainer(IContainer container) {
    BevelContainer dialogue_bv = new() {
      BevelSize = 1,
      Depth = -0.5f,
      PixelSizeY = 36.0f,
      SizeX = 0.72f,
      Anchor = new(0.0f, 1.0f),
      Offset = new(0.0f, 1.0f)
    };

    MarginContainer margin = new() {
      Margins = new(3.0f, 0.0f)
    };

    dialogue_bv.AddView(new ColorRect(DigiColor.WHITE));
    dialogue_bv.AddView(margin);
    margin.AddView(box);

    container.AddView(dialogue_bv);

    return box;
  }

  public void AttemptToPurchase(IWorldItem item) {
    // no op currently
    if (userData.Charge(item.StorePrice)) {
      box.Content = "thanks for purchasing!!!";
      userData.GetInventory().AddToInventory(item.RID);
    } else {
      box.Content = "yuo cannot afford....";
    }
  }

  public override bool HandleInput(InputType input, InputState state) {
    base.HandleInput(input, state);

    int selected_prev = menu.GetSelected();

    if (state != InputState.RELEASE) {
      if (input == InputType.UP) {
        menu.DecrementSelector();
      } else if (input == InputType.DOWN) {
        menu.IncrementSelector();
      } else if (input == InputType.CONFIRM) {
        menu.ConfirmSelector();
      }
    }

    int selected_current = menu.GetSelected();

    if (selected_prev != selected_current) {
      box.Content = store_items[selected_current].Description;
    }

    return true;
  }
}