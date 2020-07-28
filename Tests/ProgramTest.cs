using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Channels;
using System.Threading.Tasks;

using Moq;

using NUnit.Framework;

using WordCounter.ArgumentParser;
using WordCounter.Channels;
using WordCounter.Consumers;
using WordCounter.Generated;
using WordCounter.Producers;
using WordCounter.Tests.Stubs;
using WordCounter.Words;

namespace WordCounter.Tests
{
  [TestFixture]
  class ProgramTest
  {
    [Test]
    public async Task Run()
    {
      var caseInsensitiveExcludedWord = "aT";
      var wordBeginsWithA = "Ab";
      var wordBeginsWithB = "bA";
      var inWords = Enumerable.Repeat(new[] { caseInsensitiveExcludedWord, wordBeginsWithA, wordBeginsWithB }, 2).SelectMany(x => x);

      var repo = new MockRepository(MockBehavior.Strict);
      var programArgumentsMock = SetupProgramArguments(repo, inWords);
      IReadOnlyDictionary<FileNameSuffix, IWordManager> outFilesAndWords = new Dictionary<FileNameSuffix, IWordManager>();

      programArgumentsMock
        .Setup(x => x.GetDataStore.Store(It.IsAny<IReadOnlyDictionary<FileNameSuffix, IWordManager>>()))
        .Callback<IReadOnlyDictionary<FileNameSuffix, IWordManager>>(x => outFilesAndWords = x);

      await Program.Run(programArgumentsMock.Object, SetupChannelManager(repo));

      Func<string, string> expectedFileText = word => $"{CultureInfo.CurrentCulture.TextInfo.ToTitleCase(word)} 2\r\n";
      Assert.That(outFilesAndWords.Keys.Count(), Is.EqualTo(3));
      Assert.That(outFilesAndWords[FileNameSuffix.A].PrettyPrint, Is.EqualTo(expectedFileText(wordBeginsWithA)));
      Assert.That(outFilesAndWords[FileNameSuffix.B].PrettyPrint, Is.EqualTo(expectedFileText(wordBeginsWithB)));
      Assert.That(outFilesAndWords[FileNameSuffix.Exclude].PrettyPrint, Is.EqualTo(expectedFileText(caseInsensitiveExcludedWord)));
    }

    static Mock<IProgramArguments> SetupProgramArguments(MockRepository repo, IEnumerable<string> inWords)
    {
      var inData = repo.Create<IInputData>();
      var programArgumentsMock = repo.Create<IProgramArguments>();
      inData.SetupGet(x => x.GetInputData).Returns(inWords);
      programArgumentsMock.SetupGet(x => x.GetInputData).Returns(inData.Object);

      programArgumentsMock.SetupGet(x => x.GetDataStore.StoreLocation).Returns(String.Empty);
      return programArgumentsMock;
    }

    static ChannelManager SetupChannelManager(MockRepository repo)
    {
      var channel = Channel.CreateBounded<string>(10);

      var producerCreatorMock = repo.Create<IProducerCreator>();
      producerCreatorMock.Setup(x => x.Create(It.IsAny<ChannelWriter<string>>()))
        .Returns<ChannelWriter<string>>(writer => new ProducerStub(writer));

      var producerManager = new ProducerManager(2, channel.Writer, producerCreatorMock.Object);
      var producerManagerCreatorMock = repo.Create<IProducerManagerCreator>();
      producerManagerCreatorMock.Setup(x => x.Create(It.IsAny<ChannelWriter<string>>())).Returns(producerManager);

      var channelManager = new ChannelManager(channel, producerManagerCreatorMock.Object, new ConsumerManagerCreator());
      return channelManager;
    }
  }
}
