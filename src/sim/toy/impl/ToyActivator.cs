using digipet.image;

namespace digipet.sim.toy;

public interface IToyActivator : IToy {
  public bool Active { get; set; }
}

// item which activates/deactivates a toy
public class BaseToyActivator : IToyActivator {
  private readonly IToy base_item;
  private readonly BaseToyHandler handler;

  public int RID => base_item.RID;
  public string Name => base_item.Name;
  public string Description => base_item.Description;
  public int StorePrice => base_item.StorePrice;
  public ISprite SpriteOverride { get; set; }

  // expose world item attribs??

  // associating this with the relevant toy RID?

  public BaseToyActivator(
    BaseToyHandler handler,
    IToy toy
  ) {
    base_item = toy;
    this.handler = handler;

    SpriteOverride = base_item.SpriteOverride;
  }

  // this is pretty much it
  public bool Active {
    get => handler.Active;
    set => handler.Active = value;
  }
}