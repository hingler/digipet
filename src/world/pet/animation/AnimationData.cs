using System;
using System.Collections.Generic;
using System.Numerics;
using digipet.image;
using digipet.pet;
using digipet.util;
using digipet.world.pet;
using digipet.world.pet.animation;

public class AnimatedSpriteData : IAnimationData {
  private readonly IAnimatedSprite sprite;
  private readonly double frame_time;
  private double time;

  public int Frame {
    get => Math.Clamp(
      (int)Math.Floor(time / frame_time), 
      0, 
      sprite.GetFrameCount()
    );
  }

  public int FrameCount {
    get => sprite.GetFrameCount();
  }

  public double Duration {
    get => FrameCount * frame_time;
  }

  public double Progress {
    get => Math.Clamp(time, 0.0, Duration);
  }

  public PetAnimation? CurrentAnimation { get; }
  public bool Loop { get; }
  public Vector2 FaceOffsetPx { get; set; } = Vector2.Zero;


  public AnimatedSpriteData(
    IAnimatedSprite sprite,
    double frame_time,
    PetAnimation current_animation,
    bool loop
  ) {
    this.sprite = sprite;
    this.frame_time = frame_time;
    time = 0.0;
    CurrentAnimation = current_animation;
    Loop = loop;
  }

  public void Reset() {
    time = 0.0;
  }

  public void Update(double delta) {
    time += delta;
    if (time > Duration) {
      time %= Duration;
    }

    sprite.Frame = Frame;
  }

  public ISprite GetCurrentSprite() {
    return sprite;
  }

  public bool Complete() {
    return !Loop && time > Duration;
  }
}

public class AnimationData : IAnimationData {
  private readonly IList<ISprite> sprites;
  private readonly double frame_time;
  private double time;

  public PetAnimation? CurrentAnimation { get; }
  public int Frame { 
    get => Math.Clamp(
      (int)Math.Floor(time / frame_time),
      0, 
      sprites.Count
    );
  }

  public int FrameCount {
    get => sprites.Count;
  }

  public double Duration {
    get => FrameCount * frame_time;
  }

  public double Progress {
    get => Math.Clamp(time, 0.0, Duration);
  }

  public PetEmote Emote { get; set; }

  public bool Loop { get; }
  // lil bit messy but not sure how else to do it
  public Vector2 FaceOffsetPx { get; set; } = Vector2.Zero;

  public AnimationData(
    PetAnimation current_animation,
    double frame_time,
    bool loop
  ) {
    sprites = [];
    time = 0.0;
    this.frame_time = frame_time;

    CurrentAnimation = current_animation;
    Loop = loop;
  }

  public void Reset() {
    time = 0.0;
  }

  public void AddSprite(ISprite sprite) {
    sprites.Add(sprite);
  }

  public void Update(double delta) {
    time += delta;
    if (time > Duration && !Loop) {
      time %= Duration;
    }
  }

  public ISprite GetCurrentSprite() {
    int index = Math.Clamp(
      (int)Math.Floor(time / frame_time), 
      0, 
      sprites.Count
    );
    return sprites[index];
  }

  public bool Complete() {
    return !Loop && time > Duration;
  }
}