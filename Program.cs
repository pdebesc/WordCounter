using System;
using System.Threading.Tasks;

using WordCounter.ArgumentParser;
using WordCounter.Channels;

namespace WordCounter
{
  class Program
  {
    static async Task Main(string[] args)
    {
      if (!new CommandLineArgumentHandler().TryParse(args, out var programArguments))
        return;
      await Run(programArguments, ChannelManagerCreator.Create());
    }

    internal static async Task Run(
      IProgramArguments programArguments,
      IChannelManager channelManager)
    {
      var producers = channelManager.ProduceAsync(programArguments.GetInputData);
      var consumers = channelManager.ConsumeAsync();
      var result = await channelManager.GetResultAsync(producers, consumers);
      programArguments.GetDataStore.Store(result);
      Console.WriteLine($"Done: Files written to {programArguments.GetDataStore.StoreLocation}");
    }
  }
}
