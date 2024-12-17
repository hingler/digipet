using digipet.framework;
using digipet.image;
using digipet.scenes;
using digipet.user;
using digipet.view.menu.carousel;

namespace digipet.view.pet.carousel;

public class DiaryItem : ICarouselMenuItem {
  public ISprite Sprite { get; }
  public string Name { get; } = "Diary";
  public string Description { get; } = 
    "Can you please tell me all about you? I want to know more. "
    + "This is the place to let your thoughts unwind: "
    + "where every story is incredible.";

  private readonly IEngine engine;
  private readonly IUserData userdata;

  public DiaryItem(IEngine engine, IUserData userdata) {
    this.engine = engine;
    this.userdata = userdata;
    Sprite = engine.GetSpriteFetcher().GetSprite(sprite.attrib.SpriteID.OFFSET_FOOD + 2);
  }

  public void OnSelected() {
    engine.PushScene(new DiaryScene(engine));
  }
}