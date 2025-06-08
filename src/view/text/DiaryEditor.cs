using System.Numerics;
using digipet.canvas.font;
using digipet.component;
using digipet.diary;
using digipet.diary.handlers;
using digipet.diary.handlers.save;
using digipet.framework;
using digipet.input;
using digipet.sprite;
using digipet.util;
using digipet.view.text.diary;
using digipet.world.pet;
using digipet.world.pet.animation.handlers;

namespace digipet.view.text;

public class DiaryEditor : ViewComponent {
  private readonly CursorDisplayManager manager;
  private readonly Lerper offset_lerp = new(12.5);

  private readonly DiaryFooter footer;

  private readonly DiaryViewer viewer;
  private readonly List<IDiaryMetaListener> listeners;
  public bool Active = true;

  public FontType Font {
    get => viewer.Font;
    set {
      viewer.Font = value;
      manager.Font = value;
    }
  }

  public DigiColor Color {
    get => viewer.Color;
  }

  public int MarginPx {
    get => viewer.MarginPx;
    set => viewer.MarginPx = value;
  }

  public ITextEditor Editor => manager.Editor;

  public DiaryEditor(IEngine engine) : this(engine, new SimpleTextEditor()) {}

  public DiaryEditor(IEngine engine, ITextEditor editor) {
    manager = new(engine, editor);

    // how do we scroll?
    // - draw all lines

    viewer = new DiaryViewer(manager) {
      Anchor = new(0.0f, 0.0f),
      Offset = new(0.0f, 0.0f),
      HeaderLines = 3
    };

    AddView(viewer);

    // tba: stick pet view in here
    // add some logic to get input callbacks
    SimplePetSprite pet = new(engine) {
      Anchor = new(0.5f, 0.8f),
      X = 0.9f,
      Y = 0.9f,
      ZIndex = 5,
      Scale = 1.0f
    };

    IPetController controller = pet.GetController();
    controller.AddAnimationHandler(PetAnimation.IDLE, new IdleHandler(engine));
    controller.AddAnimationHandler(PetAnimation.SLEEP, new SleepHandler(engine));
    DiaryIdleTask di = new();
    DiarySleepTask ds = new();
    DiarySaveTask save = new(engine, new(0.9f, 0.8f), this);

    listeners = [ di, ds, save ];

    controller.AddTask(di);
    controller.AddTask(ds);
    controller.AddTask(save);
    
    AddView(pet);
    // - idle state

    footer = new(engine) {
      Content = "Ln 1 Col 1",
      Alignment = HorizontalAlign.LEFT,
      PixelSizeY = 16.0f,
      SizeX = 0.8f,
      X = 0.1f
    };

    AddView(footer);
    offset_lerp.Target = 0;
  }

  public void OnSaveBegin() {
    foreach (IDiaryMetaListener l in listeners) {
      l.OnSaveBegin();
    }
  }

  public void OnSaveComplete() {
    foreach (IDiaryMetaListener l in listeners) {
      l.OnSaveComplete();
    }
  }

  public override void Tick(double delta) {
    double cursor_target = PixelSizeY * 0.75;
    double current_cursor = manager.GetCursorY(-viewer.HeaderLines);

    offset_lerp.Target = Math.Max(current_cursor - cursor_target, 0.0) + 0.5;

    offset_lerp.Tick(delta);

    viewer.PageOffset = (float)offset_lerp.Cursor / manager.GetLineHeight();

  }

  public override void Reflow(ICanvas canvas) {
    base.Reflow(canvas);
    footer.PixelY = canvas.GetSizePx().Y - 14.0f;
  }

  public override void Draw(ICanvas canvas) {
    base.Draw(canvas);
    DrawCursor(canvas);
  }

  private void DrawCursor(ICanvas canvas) {
    Vector2 cursor_baseline_px = new(
      manager.GetCursorX() + MarginPx, 
      manager.GetCursorY(viewer.PageOffset - viewer.HeaderLines)
    );
    Vector2 cursor_start = cursor_baseline_px - new Vector2(0.0f, FontSizeHandler.GetFontAscent(Font));
    Vector2 cursor_end = cursor_baseline_px + new Vector2(0.0f, FontSizeHandler.GetFontDescent(Font));

    cursor_start = canvas.PxToRelative(cursor_start);
    cursor_end = canvas.PxToRelative(cursor_end);
    canvas.Line(cursor_start, cursor_end, 1.0f, new DigiColor(0.5f), z_index: 10);
  }

  public override bool HandleInput(IKeyEvent key) {
    // handle delete
    // handle return

    // handle cursor input

    // if string: type
    if (key.State == InputState.RELEASE) {
      return true;
    } else if (!Active) {
      return false;
    }

    // tba: handle modifiers

    if (key.KeyChar == string.Empty) {
      // not a text char
      if (key.Flags.HasFlag(KeyFlags.DELETE)) {
        manager.Delete();
      } else if (key.Flags.HasFlag(KeyFlags.LINE_BREAK)) {
        manager.Put(Environment.NewLine);
      } else {
        switch (key.Action) {
          case InputType.LEFT:
            if (key.Flags.HasFlag(KeyFlags.ALT)) {
              manager.SeekToPreviousWord();
            } else {
              manager.CursorLeft();
            }
            break;
          case InputType.RIGHT:
            if (key.Flags.HasFlag(KeyFlags.ALT)) {
              manager.SeekToNextWord();
            } else {
              manager.CursorRight();
            }
            break;
          case InputType.UP:
            manager.CursorUp();
            break;
          case InputType.DOWN:
            manager.CursorDown();
            break;
          default:
            return false;
            // this fall thru case is the only one we return false on
        }
      }

    } else {
      listeners.ForEach(l => l.OnCharInput());
      manager.Put(key.KeyChar);
    }

    footer.Content = "Ln " + (manager.GetLine() + 1) + " Col " + (manager.GetColumn() + 1);
    return true;
  }
}