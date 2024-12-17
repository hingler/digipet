using digipet.component;
using digipet.framework;
using digipet.input;
using digipet.user;
using digipet.util;
using digipet.view.bg;
using digipet.view.menu.carousel;
using digipet.view.pet.carousel;

namespace digipet.view.pet;

public class PetCarouselMenu : ViewComponent {

  private readonly CarouselMenu menu;

  public PetCarouselMenu(IEngine engine, IUserData data) {
    AddView(new ColorRect(DigiColor.WHITE.WithOpacity(0.75f)));
    menu = new(engine);
    AddView(menu);

    menu.AddItem(new ShopItem(engine, data));   
    menu.AddItem(new DiaryItem(engine, data)); 

    // so that we spin back to item 0 on open
    menu.Cursor = 5;
    menu.Target = 0;
  }

  public override bool HandleInput(IKeyEvent @event) {
    if (@event.State != InputState.RELEASE) {
      if (@event.Action == InputType.LEFT) {
        menu.Target--;
      } else if (@event.Action == InputType.RIGHT) {
        menu.Target++;
      } else if (@event.Action == InputType.BACK) {
        PopSelf();
      } else if (@event.Action == InputType.CONFIRM) {
        menu.Select();
        PopSelf();
      }
    }

    return true;
  }
}