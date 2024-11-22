using System;
using System.Collections.Generic;
using System.Reflection;
using digipet.file;
using digipet.file.csv;
using digipet.framework;
using digipet.sprite.attrib;
using digipet.strings;
using digipet.util;
using static digipet.util.Closure;

namespace digipet.sim.edible;

#nullable enable

public class FoodCSVConverter : ICSVConverter<IEdiblePickup> {
  int parse_count = 0;

  private readonly Dictionary<int, string> descriptions = [];

  public FoodCSVConverter(IEngine engine) {
    StringDataParser parser = new(
      engine.GetDigipetAssetLoader().Load("food_description.stringdata", FileFlags.Read).AsString()
    );

    StringDataTag? tag = parser.GetNextResult();
    while (tag != null) {
      if (tag.attributes.TryGetValue("RID", out string? rid_string)) {
        rid_string?.Let(s => descriptions.TryAdd(int.Parse(s), tag.text_content));
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
      Variance = double.Parse(args[6])
    };

    // could prob do this with reflection automagically

    return food;
  }
}