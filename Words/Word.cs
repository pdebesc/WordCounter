using System;
using System.Globalization;

namespace WordCounter.Words
{
  class Word : IWord
  {
    Func<string?, string> exceptionMessageFunc = w =>
      {
        var wordValue = w == null ? "NULL" : w == string.Empty ? "EMPTY" : w;
        return $"The value '{wordValue}' is not supported";
      };

    public Word(string word)
    {
      if (string.IsNullOrWhiteSpace(word))
        throw new ArgumentException(exceptionMessageFunc(word));
      TheWord = word;
    }

    public string TheWord { get; }

    public char FirstLetter => char.ToUpper(TheWord[0]);

    public string PrettyPrint => CultureInfo.CurrentCulture.TextInfo.ToTitleCase(TheWord);

    #region IEquatable

    public bool Equals(IWord other)
    {
      return String.Equals(TheWord, other.TheWord, StringComparison.OrdinalIgnoreCase);
    }

    public override bool Equals(object? obj)
    {
      if (ReferenceEquals(null, obj)) return false;
      if (ReferenceEquals(this, obj)) return true;
      if (obj.GetType() != this.GetType()) return false;
      return Equals((Word)obj);
    }

    public override int GetHashCode()
    {
      return StringComparer.OrdinalIgnoreCase.GetHashCode(TheWord);
    }

    #endregion

  }
}