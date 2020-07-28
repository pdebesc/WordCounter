using System.Threading.Channels;

namespace WordCounter.Consumers
{
  interface IConsumerManagerCreator
  {
    IConsumerManager Create(ChannelReader<string> reader);
  }
}