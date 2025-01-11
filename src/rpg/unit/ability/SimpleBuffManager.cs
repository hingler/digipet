using digipet.rpg.context;
using digipet.rpg.context.simple;

namespace digipet.rpg.unit.ability;

public class SimpleBuffManager {
  private readonly ICharStats base_stats;
  private ICharStats stat_cache;
  private bool cache_dirty;
  public List<ICharBuff> buff_list;

  public ICharStats Stats {
    get {
      if (cache_dirty) {
        stat_cache = UpdateStatCache();
        cache_dirty = false;
      }

      return stat_cache;
    }
  }

  public SimpleBuffManager(ICharStats base_stats) {
    this.base_stats = base_stats;
    cache_dirty = true;
    buff_list = [];
    SimpleCharStats precache = new();
    precache.Set(base_stats);

    stat_cache = precache;
  }

  public void ApplyBuff(ICharBuff buff) {
    buff_list.Add(buff);
    cache_dirty = true;
  }

  public ICharStats UpdateStatCache() {
    SimpleCharStats stats = new();
    stats.Set(base_stats);

    buff_list.RemoveAll(b => b.TimeRemaining <= 0.0001);

    foreach (ICharBuff buff in buff_list) {
      stats.Scale(buff.BuffMult);
      stats.Add(buff.BuffAdd);
    }

    return stats;
  }
}