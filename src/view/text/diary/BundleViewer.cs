using System.Numerics;
using digipet.component;
using digipet.diary.store;
using digipet.framework;
using digipet.input;
using digipet.util;

namespace digipet.view.text.diary;

public class BundleViewer : ViewComponent {

  private readonly IList<DiaryViewer> viewers = [];

  private static readonly float MARGIN_LINES = 1.0f;

  private float scroll_offset = -0.5f;

  private bool up_held = false;
  private bool down_held = false;

  private Lerper scroll_lerp = new(15.0);

  public float ScrollOffset {
    get => scroll_offset;
    set {
      scroll_offset = value;
      UpdateScrollOffsets();
    }
  }

  public BundleViewer(IEngine engine, DiaryBundle bundle) {
    scroll_lerp.Target = -0.5;
    scroll_lerp.Reset();
    foreach (IDiaryRecord record in bundle.GetRecords()) {
      DiaryViewer viewer = new(engine, record.Content) {
        HeaderText = record.CreationTime.ToString("MMM dd, yyyy"),
        MarginPx = 8,
        SizeX = 0.8f,
        X = 0.1f
      };

      viewers.Add(viewer);
      AddView(viewer);
    }

    UpdateScrollOffsets();
  }

  public override void Tick(double delta) {
    base.Tick(delta);

    double target_speed = 0.0;
    if (up_held) {
      target_speed -= 1.0;
    }

    if (down_held) {
      target_speed += 1.0;
    }

    scroll_lerp.Target = target_speed * 16.0;
    scroll_lerp.Tick(delta);
    ScrollOffset += (float)(scroll_lerp.Cursor * delta);
  }

  public override bool HandleInput(IKeyEvent @event) {
    if (base.HandleInput(@event) || @event == null) {
      return true;
    }

    bool held = (@event.State != InputState.RELEASE);

    this.GetLogger().Log("hold: ", held, ", event: ", Enum.GetName(@event.Action));

    if (@event.Action == InputType.DOWN) {
      down_held = held;
    } else if (@event.Action == InputType.UP) {
      up_held = held;
    } else if (held && @event.Action == InputType.BACK) {
      PopSelf();
    }

    return true;
  }

  private void UpdateScrollOffsets() {
    float net_offset = scroll_offset;
    for (int i = 0; i < viewers.Count; i++) {
      viewers[i].PageOffset = net_offset;
      net_offset -= (viewers[i].GetNetLineCount() + MARGIN_LINES);
    }
  }

  public override void Draw(ICanvas canvas) {
    base.Draw(canvas);
    // working well enough
    canvas.Rect(Vector2.Zero, Vector2.One, 0, DigiColor.BLACK.WithOpacity(0.5f), -25);
  }
}