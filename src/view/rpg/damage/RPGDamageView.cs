using System.Numerics;
using digipet.canvas.font;
using digipet.component;
using digipet.rpg;
using digipet.rpg.context;
using digipet.util;

namespace digipet.view.rpg.damage;

#nullable enable

public class RPGDamageView : ViewComponent, IRPGDisplay {
  private readonly LinkedList<RPGDamageNumber> numbers;

  private const double MAX_LIFETIME = 1.5;

  public Vector2 WorldOrigin { get; set; }
  public float WorldScale { get; set; }

  private static readonly ILogger logger = LoggerSingleton.GetStaticLogger<RPGDamageView>();

  private readonly Dictionary<IDetailedCharState, long> damage_trackers;
  private readonly CombatManager manager;
  private readonly Random rand;

  public RPGDamageView(CombatManager manager) {
    numbers = [];
    damage_trackers = [];
    this.manager = manager;
    rand = new();

    logger.Log("char count: ", manager.GetAllCharacters().Count());

    foreach (IDetailedCharState state in manager.GetAllCharacters()) {
      damage_trackers.Add(state, state.CurrentHP);
    }
  }

  public ViewComponent GetRootView() => this;

  public override void Tick(double delta) {
    base.Tick(delta);

    foreach (IDetailedCharState state in manager.GetAllCharacters()) {
      if (damage_trackers.TryGetValue(state, out long hp_last)) {
        if (state.CurrentHP != hp_last) {
          CreateDamageNumber(state, hp_last);
        }
      }
    }
    
    UpdateNumbers(delta);
  }

  private void UpdateNumbers(double delta) {
    LinkedListNode<RPGDamageNumber>? node = numbers.First;
    while (node != null) {
      RPGDamageNumber number = node.Value;
      float ascent = (float)(2.0 * delta);

      
      
      number.Velocity.X = (float)Lerper.LerpValue(delta, number.Velocity.X, 45.0, 0.0);
      number.Velocity.Y = (float)Lerper.LerpValue(delta, number.Velocity.Y, 15.0, 0.0);
      number.Position += number.Velocity * (float)delta + new Vector2(0.0f, ascent);
      number.Lifetime -= delta;

      node.Value = number;

      LinkedListNode<RPGDamageNumber>? next = node?.Next;

      if (number.Lifetime <= 0.0 && node != null) {
        numbers.Remove(node);
      }

      node = next;
    }
  }

  public override void Draw(ICanvas canvas) {
    base.Draw(canvas);

    foreach (RPGDamageNumber number in numbers) {
      Vector2 screenspace = this.ProjectAbsolute(number.Position) / SizePx;
      // use text nodes
      // obfuscate align, compute width 1x in item
      canvas.Text(
        screenspace, 
        number.Damage.ToString(), 
        1.0f,
        FontType.TINY, 
        HorizontalAlign.CENTER, 
        DigiColor.BLACK.WithOpacity(number.Lifetime / MAX_LIFETIME)
      );
    }
  }

  private void CreateDamageNumber(IDetailedCharState target, long hp_last) {
    long damage_delta = hp_last - target.CurrentHP;
    RPGDamageNumber number = new() {
      Position = target.Position + new Vector2(0.0f, target.Stats.Width / 2),
      Damage = damage_delta,
      Lifetime = MAX_LIFETIME
    };

    float velocity_x = (target.Team == UnitTeam.ALLY ? -45 : 45) * (rand.NextSingle() * 0.18f + 0.91f);
    float velocity_y = rand.NextSingle() * 6.0f;

    number.Velocity = new(velocity_x, velocity_y);

    numbers.AddLast(number);

    damage_trackers[target] = target.CurrentHP;
  }
}