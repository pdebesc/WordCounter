using System.Threading.Channels;

namespace WordCounter.Producers
{
  class ProducerCreator : IProducerCreator
  {
    public Producer Create(ChannelWriter<string> writer) => new WordFromFileProducer(writer);
  }
}