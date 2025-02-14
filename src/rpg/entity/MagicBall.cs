using System.Numerics;
using digipet.framework;
using digipet.image;

using digipet.rpg.context;

namespace digipet.rpg.entity;

public class MagicBall : IWorldEntity {
  private readonly double lifetime;
  private readonly double velocity;

  private double time_alive;

  private readonly ICharContext context;
  private readonly ICharState target;

  public bool Active { get; set; } = true;

  private Vector2 position_;
  public Vector2 Position => position_;

  public ISprite Sprite { get; }
  public Vector2 WorldDims => new(0.5f, 0.5f);

  public MagicBall(
    IEngine engine,
    ICharContext context,
    ICharState target
  ) {
    this.context = context;
    Sprite = engine.GetDigipetAssetLoader().LoadSprite("sprites/rpg/magicball.png");
    this.target = target;

    lifetime = 3.0;
    velocity = 25.0;
    time_alive = 0.0;

    position_ = context.Self.Position;
  }

  public void Tick(double delta) {
    if (!Active) {
      return;
    }


    Vector2 targ_position = target.Position;
    targ_position.Y += target.Stats.Width / 2;

    double travel_dist = delta * velocity;

    Vector2 travel_vector = targ_position - position_;
    float dist_remain = travel_vector.Length();

    if (travel_dist > dist_remain) {
      HandleAttack();
    } else {
      position_ += Vector2.Normalize(travel_vector) * (float)travel_dist;
      time_alive += delta;

      if (time_alive > lifetime) {
        Deactivate();
      }
    }
  }

  private void HandleAttack() {
    if (Active) {
      context.Attack(0.5, 0.25, target);
      Deactivate();
    }
  }

  private void Deactivate() {
    Active = false;
  }
}