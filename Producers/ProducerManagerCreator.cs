using System.Threading.Channels;

namespace WordCounter.Producers
{
  class ProducerManagerCreator : IProducerManagerCreator
  {
    public IProducerManager Create(ChannelWriter<string> writer)
    {
      return new ProducerManager(2, writer, new ProducerCreator());
    }
  }
}