using NUnit.Framework;

using WordCounter.Words;

namespace WordCounter.Tests.Words
{
  [TestFixture]
  class WordTest
  {
    [Test]
    public void Word_CaseInsensitiveEquals()
    {
      var wordLowerCase = "word";
      Assert.True(new Word(wordLowerCase).Equals(new Word(wordLowerCase.ToUpper())));
    }

    [Test]
    public void Word_CaseInsensitiveGetHashCode()
    {
      var wordLowerCase = "word";
      Assert.AreEqual(new Word(wordLowerCase).GetHashCode() ,new Word(wordLowerCase.ToUpper()).GetHashCode());
    }
  }
}
