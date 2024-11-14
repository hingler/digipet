using System.Runtime.Serialization;
using digipet.db;
using digipet.file.stream;
using digipet.file.stream.impl;
using digipet.util;
using NUnit.Framework.Internal;

namespace digipet.tests.db;

public class Dumdum : IStreamable<Dumdum> {
  public int d = -1;
  public static Dumdum FromStream(IInputStream stream) {
    Dumdum d = new();
    d.d = stream.ReadInt32();
    return d;
  }

  public void ToStream(IOutputStream stream) {
    stream.WriteInt32(d);
  }
}

public class DataStoreTest {
  private StreamableDataStore store = new();

  [OneTimeSetUp]
  public void Configure() {
  }

  [SetUp]
  public void SetUp() {
    LoggerSingleton.SetLogger(new ConsoleLogger(TestContext.Out, TestContext.Error));
    store = new();
  }

  [TearDown]
  public void TearDown() {
    TestContext.Out.Flush();
    TestContext.Error.Flush();
  }

  [Test]
  public void TestSimpleStore() {
    TestContext.Out.WriteLine("hello!!!");
    Dumdum d = new Dumdum();
    d.d = 121;
    store.Store("asd", d);

    Dumdum? d_2 = store.Fetch<Dumdum>("asd");
    Assert.That(d_2, Is.Not.Null);

    if (d_2 != null) {
      Assert.That(d_2.d, Is.EqualTo(121));
    }
  }

  [Test]
  public void TestStreamable() {
    ByteStream stream = new(new byte[65536]);
    
    Dumdum d = new Dumdum();
    d.d = 25;
    store.Store("asgjhd", d);

    stream.Seek(0);
    store.ToStream(stream);

    stream.Seek(0);
    StreamableDataStore s2 = StreamableDataStore.FromStream(stream);



    Dumdum? d2 = s2.Fetch<Dumdum>("asgjhd");
    Assert.That(d2, Is.Not.Null);

    if (d2 != null) {
      Assert.That(d2.d, Is.EqualTo(d.d));
    }
  }

  
}