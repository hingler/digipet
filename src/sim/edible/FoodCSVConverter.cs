using System;
using System.Collections.Generic;
using System.Reflection;
using digipet.file;
using digipet.file.csv;
using digipet.framework;
using digipet.image;
using digipet.sprite.attrib;
using digipet.strings;
using digipet.util;
using static digipet.util.Closure;

namespace digipet.sim.edible;

#nullable enable

public class FoodCSVConverter : ICSVConverter<IEdiblePickup> {
  int parse_count = 0;

  private readonly Dictionary<int, string> descriptions = [];
  private readonly Dictionary<int, ISprite> sprites = [];

  public FoodCSVConverter(IEngine engine) {
    IFileLoader loader = engine.GetDigipetAssetLoader();
    StringDataParser parser = new(
      loader.Load("food_description.stringdata", FileFlags.Read).AsString()
    );

    StringDataTag? tag = parser.GetNextResult();


    while (tag != null) {
      string? rid_string = tag.attributes.GetValueOrDefault("RID", null);
      if (int.TryParse(rid_string, out int rid)) {
        descriptions.TryAdd(rid, tag.text_content);

        if (tag.attributes.TryGetValue("Image", out string? img_path)) {
          this.GetLogger().Log("found image at: ", img_path);
          sprites.TryAdd(rid, loader.LoadSprite("sprites/food/" + img_path!));
        }
      }

      tag = parser.GetNextResult();
    }
  }

  public IEdiblePickup Parse(IList<string> args) {
    int rid = (int)SpriteID.OFFSET_FOOD + parse_count++;
    SimpleEdible food = new() {
      RID = rid,
      Name = args[0],
      Description = descriptions.GetValueOrDefault(rid) ?? args[1],
      StorePrice = int.Parse(args[2]),
      Rarity = int.Parse(args[3]),
      Satiability = double.Parse(args[4]),
      Appeal = double.Parse(args[5]),
      Variance = double.Parse(args[6]),
      SpriteOverride = sprites.GetValueOrDefault(rid) ?? null
    };

    // could prob do this with reflection automagically

    return food;
  }
}