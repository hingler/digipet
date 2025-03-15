using System.Numerics;
using digipet.file;
using digipet.framework;
using digipet.image;
using digipet.rpg;
using digipet.rpg.overworld;
using digipet.sprite.attrib;

public class NodeEntity : IWorldEntity {
  public Vector2 Position { get; set; }
  public Vector2 Velocity => Vector2.Zero;

  public bool Active => true;

  public ISprite Sprite { get; }

  public Vector2 WorldDims => Sprite.Dims * SpriteScale;

  public Vector2 SpriteScale { get; set; } = Vector2.One;

  public NodeEntity(IEngine engine, OverworldNode node) {
    IFileLoader fetcher = engine.GetDigipetAssetLoader();
    Sprite = fetcher.LoadSprite("sprites/rpg/overworld/node.png");
    Position = node.Position;
  }

  // nada
  public void Tick(double delta) {}
}