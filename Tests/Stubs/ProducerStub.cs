using System.Collections.Generic;
using System.Threading.Channels;
using System.Threading.Tasks;

using WordCounter.Producers;

namespace WordCounter.Tests.Stubs
{
  class ProducerStub : Producer
  {
    public ProducerStub(ChannelWriter<string> writer) : base(writer)
    { }

    private protected override async IAsyncEnumerable<string> ExtractItemsAsync(IEnumerator<string> items)
    {
      while (items.MoveNext())
      {
        yield return items.Current;
      }
      await Task.CompletedTask;
    }
  }
}