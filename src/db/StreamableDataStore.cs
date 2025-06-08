
using System.Reflection;
using digipet.file.stream;
using digipet.file.stream.impl;
using digipet.util;

namespace digipet.db;

#nullable enable

// when fetching objects:
// - throw exception if namespaces overlap
// - import as init requested object, if it succeeds.

// - possibly: encrypt data based on type, st only the passed type works??
//   - (down the line)
public class StreamableDataStore : IStreamable, IDataStore {
  private readonly Dictionary<string, IStreamable> store;
  private static readonly ILogger logger = LoggerSingleton.GetStaticLogger<StreamableDataStore>();

  public StreamableDataStore(IInputStream stream) {
    Dictionary<string, IStreamable> data = [];
    int item_count = stream.ReadInt32();
    logger.Log("found ", item_count, " items");
    for (int i = 0; i < item_count; i++) {
      string key = stream.ReadPascalString();

      // reading from file is no-no, at least w/o some whitelist
      // ok for now : )
      string type = stream.ReadPascalString();

      logger.Log("key ", key, " -> ", type);

      Type? t = Type.GetType(type);

      if (t == null) {
        logger.Error("Type ", type, " could not be found - skipping key ", key);
        continue;
      }

      // still reflection but feels more idiomatic
      ConstructorInfo? constructor = t.GetConstructor([ typeof(IInputStream) ]);
      if (constructor != null) {
        IStreamable? output = (IStreamable?)constructor.Invoke([ stream ]);
        if (output != null) {
          data[key] = output;
        } else {
          logger.Error("could not cast return from `FromStream` for key: ", key, ", type ", t.Name);
        }
      } else {
        logger.Error("could not find method `FromStream` for key: ", key, ", type ", t.Name);
      }
    }

    store = data;
  }

  public StreamableDataStore() : this([]) {}

  private StreamableDataStore(Dictionary<string, IStreamable> store) {
    this.store = store;
  }

  public void StoreLong(string index, long data) {
    StreamableLong stream_long = new(data);
    Store(index, stream_long);
  }

  public bool TryFetchLong(string index, out long data) {
    bool fetch_success = TryFetch(index, out StreamableLong? output);
    if (fetch_success) {
      data = output!.data; 
    } else {
      data = -1;
    }

    return fetch_success;
  }

  public void Store(string index, object data) {
    if (data is IStreamable streamable) {
      store[index] = streamable;
    } else {
      logger.Error("attempted to store non-streamable data type - ignoring...");
    }
  }

  public T? Fetch<T>(string index) where T : class {
    if (store.TryGetValue(index, out IStreamable? item)) {
      return item as T;
    }

    return null;
  }

  public bool TryFetch<T>(string index, out T? output) where T : class {
    bool res = store.TryGetValue(index, out IStreamable? item);
    output = item as T;
    return res;
  }

  public bool Contains<T>(string index) {
    if (store.TryGetValue(index, out IStreamable? item)) {
      return item is T;
    }

    return false;
  }

  public IDataStore GetSubspace(string prefix) {
    return new SubDataStore(prefix + ".", this);
  }

  public void ToStream(IOutputStream stream) {
    // write the number of elements we have
    logger.Log("writing ", store.Count, " items");
    stream.WriteInt32(store.Count);

    foreach (KeyValuePair<string, IStreamable> pair in store) {
      stream.WritePascalString(pair.Key);
      stream.WritePascalString(pair.Value.GetType().AssemblyQualifiedName);
      // write to a byte stream, then get the length of said stream, then prepend the length of the stream
      pair.Value.ToStream(stream);
    }
  }
}