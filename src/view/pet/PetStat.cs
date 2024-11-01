using System;
using System.Numerics;
using digipet.component;
using digipet.image;

namespace digipet.view.pet;

// displays a single pet stat
public class PetStat : ViewComponent {
  private readonly CompoundView base_view = new();

  private readonly SpriteView image;
  private readonly DigiProgressBar prog;

  const int MARGIN_WIDTH = 4;
  const int IMAGE_SIZE = 16;

  public float Fill {
    get => prog.Fill;
    set => prog.Fill = value;
  }

  public ISprite Icon {
    get => image.sprite;
    set => image.sprite = value;
  }

  public PetStat() : this(null) {}

  public PetStat(ISprite sprite) : base() {
    image = new();
    prog = new();

    image.sprite = sprite;

    base_view.AddView(image);
    base_view.AddView(prog);

    image.Anchor = new(0.0f, 0.5f);
    prog.Anchor = new(0.0f, 0.5f);
  }

  public override void Draw(ICanvas canvas) {
    Vector2 dims = canvas.GetSizePx();
    Vector2 image_size = canvas.PxToRelative(IMAGE_SIZE, IMAGE_SIZE);
    Vector2 margin_size = canvas.PxToRelative(MARGIN_WIDTH, MARGIN_WIDTH);

    image.Offset = new(0.0f, 0.5f);
    image.Size = image_size;

    prog.Offset = new(margin_size.X + image_size.X, 0.5f);
    prog.Size = canvas.PxToRelative(
      (int)dims.X - (MARGIN_WIDTH + IMAGE_SIZE), 
      8
    );

    // image is 16px
    // padding is some # (8)
    // bar is (remaining space - 24) wide, 12 tall
    // anchor popint of (0.0, 0.5)
    // gonna just handle layout here lole

    base_view.PreDraw(canvas);
  }

  // in the parent container
}