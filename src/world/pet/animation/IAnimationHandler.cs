namespace digipet.world.pet.animation;

// my thought:
// - handler decides which animation data to enqueue
// - animation state -> animation handler
// - animation handler decides whether to loop

public interface IAnimationHandler {
  IAnimationData GetAnimationData(
    PetAnimation animation
  );
}