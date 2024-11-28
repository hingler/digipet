using digipet.image;

namespace digipet.view.menu.carousel;

public interface ICarouselMenuItem {
  ISprite Sprite { get; }
  string Name { get; }
  string Description { get; }

  // called when menu itrem is selected
  void OnSelected();

}
