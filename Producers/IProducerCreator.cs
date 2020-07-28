using System.Threading.Channels;

namespace WordCounter.Producers
{
  interface IProducerCreator
  {
    Producer Create(ChannelWriter<string> writer);
  }
}