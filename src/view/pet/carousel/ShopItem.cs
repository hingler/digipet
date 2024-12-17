using digipet.framework;
using digipet.image;
using digipet.scenes;
using digipet.user;
using digipet.view.menu.carousel;

namespace digipet.view.pet.carousel;

public class ShopItem : ICarouselMenuItem {
  public ISprite Sprite { get; }
  public string Name { get; } = "Shop";
  public string Description { get; } = 
    "All of your tremendous work has been building to this. "
    + "Your favorite new passion is waiting to find you. "
    + "This is the Shopping world promise.";


  private readonly IEngine engine;
  private readonly IUserData userdata;

  public ShopItem(IEngine engine, IUserData userdata) {
    this.engine = engine;
    this.userdata = userdata;
    Sprite = engine.GetSpriteFetcher().GetSprite(sprite.attrib.SpriteID.OFFSET_FOOD + 1);
  }

  public void OnSelected() {
    engine.PushScene(new StoreScene(engine, userdata));
  }
}