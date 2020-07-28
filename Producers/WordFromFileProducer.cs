using System.Collections.Generic;
using System.Threading.Channels;

using WordCounter.Utilities;

namespace WordCounter.Producers
{
  class WordFromFileProducer : Producer
  {
    public WordFromFileProducer(ChannelWriter<string> writer) : base(writer)
    { }

    private protected override async IAsyncEnumerable<string> ExtractItemsAsync(IEnumerator<string> items)
    {
      while (items.MoveNext())
        await foreach (var item in WordReader.GetWordsAsync(items.Current))
          yield return item;
    }
  }
}