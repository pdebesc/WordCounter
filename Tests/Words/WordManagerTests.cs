using NUnit.Framework;

using WordCounter.Words;

namespace WordCounter.Tests.Words
{
  [TestFixture]
  class WordManagerTests
  {
    [Test]
    public void Add_IncreaseCounter()
    {
      var wordManager = new WordManager();
      Assert.That(wordManager.PrettyPrint, Is.Empty);

      var word = "abs";
      var count = 1;
      AssertCount(wordManager, word, count);
      AssertCount(wordManager, word, ++count);
    }

    static void AssertCount(WordManager wordManager, string wordToAdd, int count)
    {
      var word1 = new Word(wordToAdd);
      wordManager.Add(word1);
      Assert.That(wordManager.PrettyPrint, Contains.Substring($"{word1.PrettyPrint} {count}"));
    }
  }
}
