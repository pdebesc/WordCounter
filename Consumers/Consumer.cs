using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Channels;
using System.Threading.Tasks;
using WordCounter.Generated;
using WordCounter.Words;

namespace WordCounter.Consumers
{
  class Consumer
  {
    readonly ChannelReader<string> reader;
    readonly ConcurrentDictionary<FileNameSuffix, IWordManager> dictionary;
    readonly ImmutableHashSet<IWord> excludedWords;

    public Consumer(
      ChannelReader<string> reader,
      ConcurrentDictionary<FileNameSuffix, IWordManager> dictionary,
      ImmutableHashSet<IWord> excludedWords)
    {
      this.reader = reader;
      this.dictionary = dictionary;
      this.excludedWords = excludedWords;
    }

    public async Task Consume()
    {
      while (await reader.WaitToReadAsync())
        while (reader.TryRead(out var item))
          DoWork(item);
    }

    void DoWork(string item)
    {
      var word = new Word(item);
      var suffix = GetFileNameSuffix(word, excludedWords);

      dictionary.AddOrUpdate(suffix, nameSuffix =>
        {
          var manager = new WordManager();
          manager.Add(word);
          return manager;
        }, (nameSuffix, manager) =>
        {
          manager.Add(word);
          return manager;
        });
    }

    internal static FileNameSuffix GetFileNameSuffix(IWord word, ICollection<IWord> excludeWords)
    {
      if (excludeWords.Contains(word))
        return FileNameSuffix.Exclude;
      return (FileNameSuffix)word.FirstLetter;
    }
  }
}