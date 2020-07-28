using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Channels;
using System.Threading.Tasks;

using WordCounter.Generated;
using WordCounter.Words;

namespace WordCounter.Consumers
{
  class ConsumerManager : IConsumerManager
  {
    readonly int numberOfConsumers;
    readonly ChannelReader<string> reader;
    readonly ImmutableHashSet<IWord> excludeWords;

    readonly ConcurrentDictionary<FileNameSuffix, IWordManager> fileNameSuffixAndWords =
      new ConcurrentDictionary<FileNameSuffix, IWordManager>();

    public ConsumerManager(int numberOfConsumers, ChannelReader<string> reader, ImmutableHashSet<IWord> excludeWords)
    {
      this.numberOfConsumers = numberOfConsumers;
      this.reader = reader;
      this.excludeWords = excludeWords;
    }

    public IEnumerable<Task> ConsumeAsync()
    {
      var consumers = new List<Task>(numberOfConsumers);
      for (var i = 0; i < numberOfConsumers; i++)
      {
        var id = i;
        var task = Task.Run(async () =>
          {
            var consumer = new Consumer(reader, fileNameSuffixAndWords, excludeWords);
            await consumer.Consume();
          });
        consumers.Add(task);
      }

      return consumers;
    }

    public IReadOnlyDictionary<FileNameSuffix, IWordManager> GetFinishedWork()
    {
      if(reader.Completion.IsCompleted)
         return fileNameSuffixAndWords;

      throw new Exception($"Make sure the channel is closed before getting the finished work.");
    }
  }
}