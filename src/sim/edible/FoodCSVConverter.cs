using System;
using System.Collections.Generic;
using System.Reflection;
using digipet.file.csv;
using digipet.sprite.attrib;

namespace digipet.sim.edible;

public class FoodCSVConverter : ICSVConverter<IEdiblePickup> {
  int parse_count = 0;

  public IEdiblePickup Parse(IList<string> args) {
    SimpleEdible food = new() {
      RID = (int)SpriteID.OFFSET_FOOD + parse_count++,
      Name = args[0],
      Description = args[1],
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