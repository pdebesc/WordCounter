using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace WordCounter
{
  class Program
  {
    static async Task Main(string[] args)
    {
      var testPath = @"C:\Users\z6pdc\Desktop\ToDelete\TestFiles";
      //var txtFiles = Directory.EnumerateFiles(testPath, "*.txt", SearchOption.TopDirectoryOnly).ToList();
      var txtFiles = new DirectoryInfo(testPath).EnumerateFiles("*.txt", SearchOption.TopDirectoryOnly).OrderByDescending(x => x.Length).Select(x => x.FullName);




      var boundedChannelOptions = new BoundedChannelOptions(2) { FullMode = BoundedChannelFullMode.Wait };
      var channel = Channel.CreateBounded<string>(boundedChannelOptions);

      var cancellationTokenSource = new CancellationTokenSource();
      var cancellationToken = cancellationTokenSource.Token;

      var writer = channel.Writer;
      var producer = new Producer(writer, cancellationToken);

      var taskProducer = Task.Run(
        async () =>
          {
            foreach (var txtFile in txtFiles)
            {
              await ExtractFile(txtFile, producer);
            }
          });

      var consumer = new Consumer(channel.Reader, cancellationToken,1);

      var taskConsumer = Task.Run(
        async () =>
          {
            await foreach (var item in consumer.DequeueAll())
            {
              Console.WriteLine($"Consumed {item}");
            }
          });

      Task.WaitAll(taskProducer);
      writer.Complete();

      Task.WaitAll(taskConsumer);


      Console.ReadLine();
    }

    static async Task ExtractFile(string txtFile, Producer producer)
    {
      using (var memoryMappedFile = MemoryMappedFile.CreateFromFile(txtFile))
      {
        using (var stream = memoryMappedFile.CreateViewStream())
        {
          using (var sr = new StreamReader(stream))
          {
            while (!sr.EndOfStream)
            {
              var line = sr.ReadLine();
              var strings = line.Split(" ");
              foreach (var s in strings)
              {
                await producer.Enqueue(s);
              }
            }
          }
        }
      }
    }
  }

  class Consumer
  {
    private ChannelReader<string> reader;
    private CancellationToken cancellationToken;
    readonly int id;

    public Consumer(ChannelReader<string> reader, CancellationToken cancellationToken, int id)
    {
      this.reader = reader;
      this.cancellationToken = cancellationToken;
      this.id = id;
    }

    //public async Task<string> Dequeue(Action<ValueTask<string>> consumerAction) 
    //{
    //  while (await reader.WaitToReadAsync(cancellationToken))
    //  {
    //    var readAsync = reader.ReadAsync(cancellationToken);
    //    consumerAction(readAsync);
    //  }
    //  return Task.FromResult("")
    //}

    public async IAsyncEnumerable<string> DequeueAll()
    {
      await foreach (var item in reader.ReadAllAsync())
      {
        yield return item;
      }
      //return reader.ReadAllAsync();
      //while (await reader.WaitToReadAsync(cancellationToken))
      //{
      //  await reader.ReadAllAsync(cancellationToken);
      //}

      //  return AsyncEnumerable.Empty<string>();
      //}
    }
  }

  class Producer
  {
    readonly ChannelWriter<string> writer;
    readonly CancellationToken token;

    public Producer(ChannelWriter<string> writer, CancellationToken token)
    {
      this.writer = writer;
      this.token = token;
    }

    public async Task Enqueue(string item)
    {
      while (await writer.WaitToWriteAsync(token))
      {
        await writer.WriteAsync(item, token);
        Console.WriteLine($"Producer Enqueue item {item}");
        break;
      }
    }
  }
}
