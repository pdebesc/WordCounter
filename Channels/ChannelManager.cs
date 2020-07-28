using System.Collections.Generic;
using System.Threading.Channels;
using System.Threading.Tasks;

using WordCounter.ArgumentParser;
using WordCounter.Consumers;
using WordCounter.Generated;
using WordCounter.Producers;
using WordCounter.Words;

namespace WordCounter.Channels
{
  class ChannelManager : IChannelManager
  {
    readonly Channel<string, string> channel;
    readonly IProducerManager producerManager;
    readonly IConsumerManager consumerManager;

    public ChannelManager(Channel<string, string> channel, IProducerManagerCreator producerManagerCreator, IConsumerManagerCreator consumerManagerCreator)
    {
      this.channel = channel;
      producerManager = producerManagerCreator.Create(channel.Writer);
      consumerManager = consumerManagerCreator.Create(channel.Reader);
    }

    public IEnumerable<Task> ProduceAsync(IInputData inputData)
    {
      return producerManager.ProduceAsync(inputData);
    }

    public IEnumerable<Task> ConsumeAsync()
    {
      return consumerManager.ConsumeAsync();
    }

    public async Task<IReadOnlyDictionary<FileNameSuffix, IWordManager>> GetResultAsync(IEnumerable<Task> producers, IEnumerable<Task> consumers)
    {
      await Task.WhenAll(producers);
      channel.Writer.Complete();
      await Task.WhenAll(consumers);

      return consumerManager.GetFinishedWork();
    }
  }
}