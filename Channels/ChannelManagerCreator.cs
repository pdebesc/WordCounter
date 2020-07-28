using System.Threading.Channels;

using WordCounter.Consumers;
using WordCounter.Producers;

namespace WordCounter.Channels
{
  static class ChannelManagerCreator
  {
    public static IChannelManager Create()
    {
      var boundedChannelOptions = new BoundedChannelOptions(5) { FullMode = BoundedChannelFullMode.Wait };
      var channel = Channel.CreateBounded<string>(boundedChannelOptions);
      return new ChannelManager(channel, new ProducerManagerCreator(), new ConsumerManagerCreator());
    }
  }
}