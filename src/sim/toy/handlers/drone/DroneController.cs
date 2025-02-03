using System.Numerics;
using System.Runtime.InteropServices;
using digipet.src.sim.toy.handlers;
using digipet.transition.animator;
using digipet.util;
using digipet.world;

namespace digipet.sim.toy.handlers;

public class DroneController : BaseToyHandler {
  private readonly IPhysWorld world;

  private readonly IPhysObject drone;

  private readonly Lerper thrust_control = new(15.0);

  public float MaxVelocity = 0.15f;

  private readonly TargetController targetter;

  public float Thrust => (float)thrust_control.Cursor;

  public DroneController(
    IPhysWorld world,
    IPhysObject drone
  ) {
    this.world = world;
    this.drone = drone;

    thrust_control.Target = 0.0;
    thrust_control.Reset();

    targetter = new(drone.Position.X);
  }

  public override void Tick(double delta) {
    thrust_control.Tick(delta);
    Vector2 net_force = -world.Gravity * (float)thrust_control.Cursor;
    targetter.Tick(delta, drone);

    Vector2 target = targetter.Target;
    Vector2 dist = target - drone.Position;
    Vector2 dir = Vector2.Normalize(dist);

    Vector2 current_velocity = drone.Velocity;
    float velocity_scalar = current_velocity.Length();
    float approach_speed = Vector2.Dot(dir, current_velocity);
    float approach_dist = dist.Length();

    float max_velocity = approach_dist / 1.8f;

    if (velocity_scalar > max_velocity) {
      // slow down the vessel
      net_force += Vector2.Normalize(current_velocity) * -MaxVelocity;
    } 
    
    if (approach_speed > max_velocity) {
      // push away net force
      net_force -= dir * MaxVelocity;
    } else if (approach_speed < MaxVelocity) {
      net_force += dir * MaxVelocity;
    }

    drone.ApplyForce(net_force, (float)delta);

    drone.FlipX = (drone.Velocity.X < 0.0);
  }



  protected override void Activate() {
    thrust_control.Target = 1.0;
  }

  protected override void Deactivate() {
    thrust_control.Target = 0.0;
  }

  public override void Destroy() {
    world.RemovePhysObject(drone);
  }
}