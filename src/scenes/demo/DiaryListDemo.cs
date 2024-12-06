using System.Text;
using digipet.component;
using digipet.diary.store;
using digipet.framework;
using digipet.sprite.attrib;
using digipet.view.bg;
using digipet.view.text.diary;

namespace digipet.scenes.demo;

public class DiaryListDemo : Scene {
  private DiaryList list;

  public DiaryListDemo(IEngine engine) : base(engine) {}

  public override void InitScene() {

    PushToStack(
      new ScrollingBG(
        Engine.GetSpriteFetcher().GetSprite(SpriteID.OFFSET_BG)
      )
    );
    list = new(Engine) {};

    Random r = new();

    for (int i = 0; i < 16; i++) {
      DiaryBundle b = new();

      StringBuilder s = new();
      for (int k = 0; k < 3; k++) {
        for (int j = 0; j < 32; j++) {
          s.AppendLine(r.NextDouble().ToString());
        }
        
        StreamableDiaryRecord d = new(s.ToString(), DateTime.Now);
        b.AddEntry(d);

      }


      list.AddBundle("0" + (i >= 10 ? i : "0" + i), b);
    }

    PushToStack(list);
  }
}