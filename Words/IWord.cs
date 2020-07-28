using System;

namespace WordCounter.Words
{
  interface IWord : IEquatable<IWord>, IPrettyPrint
  {
    string TheWord { get; }

    char FirstLetter { get; }
  };
}