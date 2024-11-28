using digipet.image;
using digipet.view.menu.carousel;

namespace digipet.view.menu.carousel;

public class StubMenuItem : ICarouselMenuItem {
  public string Name { get; set; }
  public string Description => "HOW ARE YOU DOING";
  public ISprite Sprite { get; set; } = null;

  public StubMenuItem(string name) {
    Name = name;
  }

  public void OnSelected() { /* no op */ }
}