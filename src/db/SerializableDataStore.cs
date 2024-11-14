using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using digipet.file;
using digipet.util;
using static digipet.util.Closure;

namespace digipet.db;

#nullable enable

public class SerializableDataStore : IDataStore {
  
  private readonly Dictionary<string, object> data = []; 

  // different dicts for doubles, ints, etc??
  // i think we just sample it normally

  // guarantee newline for now
  // if this isn't newline, then read will break
  private static readonly char CHAR_SEPARATOR = '\n';
  private static readonly string KEY_LIST = "__keys";

  private readonly ILogger logger;

  public SerializableDataStore() {
    logger = this.GetLogger();
  }

  private string Sanitize(string index) {
    if (!index.Contains(CHAR_SEPARATOR)) {
      return index.Replace(CHAR_SEPARATOR, '_');
    }

    return index;
  }

  public void Store(string index, object data) {
    this.data[Sanitize(index)] = data;
  }

  public T? Fetch<T>(string index) where T : class {
    if (data.TryGetValue(Sanitize(index), out object? val)) {
      return val as T;
    }

    return null;
  }

  public IDataStore GetSubspace(string prefix) {
    return new SubDataStore(prefix + ".", this);
  }

  // want some type that can represent a k/v store
  public SerializableDataStore(SerializationInfo info, StreamingContext context) : this() {
    string? key_list = info.GetString(KEY_LIST);
    key_list?.Let(s => {
      HandleKeys(info, s);
    });
  }

  private void HandleKeys(SerializationInfo info, string keys) {
    StringReader reader = new(keys);
    string? str = reader.ReadLine();
    while (str != null) {
      object? val = info.GetValue(str, typeof(object));
      if (val != null) {
        data[str] = val;
      } else {
        logger.Error("invalid data at key: ", str);
      }

      str = reader.ReadLine();
    }
    
  }

  public void GetObjectData(SerializationInfo info, StreamingContext context) {
    StringBuilder key_list = new();
    foreach (KeyValuePair<string, object> kv in data) {
      info.AddValue(kv.Key, kv.Value, kv.Value.GetType());
      key_list.Append(kv.Key);
      key_list.Append(CHAR_SEPARATOR);
    }

    info.AddValue(KEY_LIST, key_list.ToString());
  }
}