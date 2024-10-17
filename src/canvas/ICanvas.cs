using System.Numerics;

namespace digipet;

public interface ICanvas {
  // draw a colored rect onto the screen
  void DrawRect(Vector2 start, Vector2 end, Vector4 col);

  // draw text onto the screen
  void DrawText(Vector2 origin, string text);

  // tba: drawing images?

  // called at the end of a "draw frame"
  void Flush();
}