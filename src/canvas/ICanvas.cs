using System.Numerics;

namespace digipet;

public interface ICanvas {
  // draw a colored rect onto the screen
  void Rect(Vector2 start, Vector2 end, Vector4 col);

  // draw text onto the screen
  void Text(Vector2 origin, string text, float scale);

  // tba: drawing images?

  // called at the end of a "draw frame"
  void Flush();
}