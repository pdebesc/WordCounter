using NUnit.Framework;

using WordCounter.Consumers;
using WordCounter.Generated;
using WordCounter.Words;

namespace WordCounter.Tests.Consumers
{
  [TestFixture]
  class ConsumerTest
  {
    [Test]
    public void GetFileNameSuffix_Excluded()
    {
      var s = "ExcludeMe";
      var excludeWords = new []{new Word(s) };
      var fileNameSuffix = Consumer.GetFileNameSuffix(new Word(s), excludeWords);
      Assert.That(fileNameSuffix, Is.EqualTo(FileNameSuffix.Exclude));
    }

    [Test]
    public void GetFileNameSuffix_NotExcluded()
    {
      var s = "ExcludeMe";
      var excludeWords = new[] { new Word(s) };
      var fileNameSuffix = Consumer.GetFileNameSuffix(new Word($"Not{s}"), excludeWords);
      Assert.That(fileNameSuffix, Is.EqualTo(FileNameSuffix.N));
    }
  }
}
