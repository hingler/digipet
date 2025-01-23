using digipet.image;

namespace digipet.sim.toy;

public interface IToyActivator : IWorldItem {
  public bool Active { get; set; }
}

// item which activates/deactivates a toy
public class BaseToyActivator : IToyActivator {
  private readonly IWorldItem base_item;
  private readonly BaseToyHandler handler;

  public int RID => base_item.RID;
  public ISprite SpriteOverride => base_item.SpriteOverride;

  public BaseToyActivator(
    BaseToyHandler handler,
    IWorldItem item
  ) {
    base_item = item;
    this.handler = handler;
  }

  // this is pretty much it
  public bool Active {
    get => handler.Active;
    set => handler.Active = value;
  }
}