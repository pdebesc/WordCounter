using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Reflection;

using WordCounter.Words;

namespace WordCounter.Utilities
{
  static class EmbeddedFileUtils
  {
    public static ImmutableHashSet<IWord> GetExcludedWords()
    {
      var words = new List<IWord>();
      var assembly = Assembly.GetExecutingAssembly();
      var resourceName = assembly.GetManifestResourceNames()
        .Single(str => str.EndsWith("exclude.txt"));
      using (var stream = assembly.GetManifestResourceStream(resourceName))
      {
        using var reader = new StreamReader(stream);
        while (!reader.EndOfStream)
        {
          var toExclude = reader.ReadLine().Trim();
          var word = new Word(toExclude);
          words.Add(word);
        }
      }

      return ImmutableHashSet.Create(words.ToArray());
    }
  }
}
