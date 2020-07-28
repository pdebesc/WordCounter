using System.Threading.Channels;

using WordCounter.Utilities;

namespace WordCounter.Consumers
{
  class ConsumerManagerCreator : IConsumerManagerCreator
  {
    public IConsumerManager Create(ChannelReader<string> reader)
    {
      return new ConsumerManager(2, reader, EmbeddedFileUtils.GetExcludedWords());
    }
  }
}