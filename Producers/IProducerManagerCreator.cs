using System.Threading.Channels;

namespace WordCounter.Producers
{
  interface IProducerManagerCreator
  {
    IProducerManager Create(ChannelWriter<string> writer);
  }
}