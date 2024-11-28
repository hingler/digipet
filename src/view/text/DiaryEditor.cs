using System.Numerics;
using digipet.canvas.font;
using digipet.component;
using digipet.diary;
using digipet.framework;
using digipet.input;
using digipet.util;
using digipet.view.text.diary;

namespace digipet.view.text;

public class DiaryEditor : ViewComponent, IInputListener {
  private readonly CursorDisplayManager manager;
  private readonly FlowText content;

  private readonly Lerper offset_lerp = new(12.5);

  private readonly DiaryFooter footer;

  public FontType Font {
    get => content.Font;
    set {
      content.Font = value;
      manager.Font = value;
    }
  }

  public DigiColor Color {
    get => content.Color;
  }

  private int margin_px_ = 0;
  public int MarginPx {
    get => margin_px_;
    set {
      margin_px_ = value;
      QueueReflow();
    }
  }

  public DiaryEditor(IEngine engine) {
    manager = new(engine);

    // how do we scroll?
    // - draw all lines

    content = new FlowText(manager.Flow) {
      Anchor = new(0.0f, 0.0f),
      Offset = new(0.0f, 0.0f)
    };

    AddView(content);

    footer = new(engine) {
      Content = "Ln 1 Col 1",
      Alignment = HorizontalAlign.RIGHT,
      PixelSizeY = 16.0f,
      SizeX = 0.9f
    };

    AddView(footer);

    // best way to handle text input?
    // also: make this def input handling i think
    engine.GetInputManager().Register(this);
    offset_lerp.Target = 0;
  }

  public override void Tick(double delta) {
    double cursor_target = PixelSizeY * 0.75;
    double current_cursor = manager.GetCursorY(0.0f);

    offset_lerp.Target = Math.Max(current_cursor - cursor_target, 0.0) - 0.5;

    offset_lerp.Tick(delta);

    content.DisplayedLineOffset = (float)offset_lerp.Cursor / manager.Flow.GetLineHeight();

  }

  // if unicode, then ignore
  // treat confirm as return
  // treat 
  public void OnInput(InputType type, InputState state) { /* no op */ }

  public override void Reflow(ICanvas canvas) {
    base.Reflow(canvas);

    content.PixelSizeX = canvas.GetSizePx().X - 2 * MarginPx;
    content.PixelX = MarginPx;

    footer.PixelY = canvas.GetSizePx().Y - 14.0f;
  }

  public override void Draw(ICanvas canvas) {
    base.Draw(canvas);
    DrawCursor(canvas);

    for (int i = 0; i < 16; i++) {
      float baseline = manager.Flow.GetTextBaseline(i, content.DisplayedLineOffset % 1.0f);
      DrawBaseline(canvas, baseline);
    }
  }

  private void DrawCursor(ICanvas canvas) {
    Vector2 cursor_baseline_px = new(
      manager.GetCursorX() + MarginPx, 
      manager.GetCursorY(content.DisplayedLineOffset)
    );
    Vector2 cursor_start = cursor_baseline_px - new Vector2(0.0f, FontSizeHandler.GetFontAscent(Font));
    Vector2 cursor_end = cursor_baseline_px + new Vector2(0.0f, FontSizeHandler.GetFontDescent(Font));

    cursor_start = canvas.PxToRelative(cursor_start);
    cursor_end = canvas.PxToRelative(cursor_end);

    canvas.Line(cursor_start, cursor_end, 1.0f, new DigiColor(0.5f));
  }

  private void DrawBaseline(ICanvas canvas, float base_px) {
    Vector2 start = canvas.PxToRelative(0.0f, base_px);
    Vector2 end = canvas.PxToRelative(canvas.GetSizePx().X, base_px);

    canvas.Line(start, end, 1.0f, DigiColor.BLACK.WithOpacity(0.25f), 1.0f, -1);
  }

  public void OnKey(IKeyEvent key) {
    // handle delete
    // handle return

    // handle cursor input

    // if string: type
    if (key.State == InputState.RELEASE) {
      return;
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
            manager.CursorLeft();
            break;
          case InputType.RIGHT:
            manager.CursorRight();
            break;
          case InputType.UP:
            manager.CursorUp();
            break;
          case InputType.DOWN:
            manager.CursorDown();
            break;
        }
      }
    } else {
      manager.Put(key.KeyChar);
    }

    footer.Content = "Ln " + (manager.GetLine() + 1) + " Col " + (manager.GetColumn() + 1);
  }
}