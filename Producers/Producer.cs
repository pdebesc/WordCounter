using System.Collections.Generic;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace WordCounter.Producers
{
  abstract class Producer
  {
    readonly ChannelWriter<string> writer;

    protected Producer(ChannelWriter<string> writer)
    {
      this.writer = writer;
    }

    public Task AddAsync(IEnumerator<string> items)
    {
      var words = ExtractItemsAsync(items);
      return AddToChannelAsync(words);
    }

    async Task AddToChannelAsync(IAsyncEnumerable<string> words)
    {
      await foreach (var word in words)
        await writer.WriteAsync(word);
    }

    private protected abstract IAsyncEnumerable<string> ExtractItemsAsync(IEnumerator<string> items);
  }
}