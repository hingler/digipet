// scrolling pencils in BG
// view -> show books
// create -> go to editor
// - editor needs a file window
//   - thinking: wrap the editor and just block when escape is hit?
// - also: scene should be responsible for managing diary data - we'll figure that out

using digipet.component;
using digipet.db;
using digipet.diary.store;
using digipet.framework;
using digipet.input;
using digipet.sim;
using digipet.sprite.attrib;
using digipet.view.bg;
using digipet.view.container;
using digipet.view.diary;
using digipet.view.menu;

namespace digipet.scenes;

#nullable enable

public class DiaryScene : Scene {
  private readonly DiaryRepo repo;
  private readonly SimProvider provider;

  private static readonly string REPO_NAME = "diary_repo";

  public DiaryScene(
    IEngine engine,
    SimProvider provider
  ) : base(engine) {
    this.provider = provider;
    IDataStore store = engine.GetSaveStore();
    if (!store.TryFetch(REPO_NAME, out DiaryRepo? repo_local)) {
      repo_local = new DiaryRepo();
      store.Store(REPO_NAME, repo_local);
    }

    repo = repo_local!;

    // simprovider
  }

  public override void InitScene() {

    PushToStack(
      new ScrollingBG(
        Engine.GetSpriteFetcher().GetSprite(SpriteID.OFFSET_BG)
      )
    );

    PushToStack(
      new DiaryMenu(Engine, repo, provider)
    );
  }

  public override bool HandleInput(IKeyEvent @event) {
    if (@event.Action == InputType.BACK && @event.State == InputState.PRESS) {
      Finish();
    }

    return true;
  }
}