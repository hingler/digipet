using digipet.component;
using digipet.framework;
using digipet.sprite.attrib;
using digipet.util;

namespace digipet.view.text.diary;

public class DiaryCover : ViewComponent {
  private readonly SpriteView cover;
  private readonly Text text;
  
  public string Content {
    get => text.Content;
    set => text.Content = value;
  }

  public DiaryCover(IEngine engine) : base() {
    cover = new(engine.GetSpriteFetcher().GetSprite(SpriteID.DIARY_COVER));
    // 4L + 2R
    text = new() {
      Font = canvas.font.FontType.SMALL,
      Color = DigiColor.BLACK
    };

    AddView(cover);
    AddView(text);

    // associate here??

    SizePx = cover.Sprite.Dims;
    text.OffsetPx = new(SizePx.X / 2.0f + 1.0f, SizePx.Y / 2.0f + 5.0f);
    text.Alignment = HorizontalAlign.CENTER;
  }
}