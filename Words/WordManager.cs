using System.Collections.Concurrent;
using System.Text;

namespace WordCounter.Words
{
  class WordManager : IWordManager
  {
    ConcurrentDictionary<IWord, int> wordsAndCount = new ConcurrentDictionary<IWord, int>();

    public void Add(IWord word)
    {
      wordsAndCount.AddOrUpdate(word, 1, (w, count) => ++count);
    }

    public string PrettyPrint
    {
      get
      {
        var sb = new StringBuilder();
        foreach (var kvp in wordsAndCount)
          sb.Append(kvp.Key.PrettyPrint).Append(" ").Append(kvp.Value).AppendLine();

        return sb.ToString();
      }
    }
  }
}