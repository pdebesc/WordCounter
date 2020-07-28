using System;
using System.Collections.Generic;
using System.IO;

using Moq;

using NUnit.Framework;

using WordCounter.Generated;
using WordCounter.Store;
using WordCounter.Words;

namespace WordCounter.Tests.Store
{
  [TestFixture]
  class FileStoreTest
  {
    [Test]
    public void CreateFileNameTest()
    {
      var name = Enum.GetName(typeof(FileNameSuffix), FileNameSuffix.T);
      Assert.That(FileStore.CreateFileName(name), Is.EqualTo("FILE_T.txt"));
    }

    [Test]
    public void WriteToFileTest()
    {
      var mockRepository = new MockRepository(MockBehavior.Strict);

      var writer = mockRepository.Create<TextWriter>();
      writer.Setup(x => x.Write(It.IsAny<string?>())).Verifiable();
      writer.As<IDisposable>().Setup(x => x.Dispose());

      var textWriterCreatorMock = mockRepository.Create<ITextWriterCreator>();
      textWriterCreatorMock.Setup(x => x.Create(It.IsAny<string>())).Returns(writer.Object);

      var prettyPrints = new Dictionary<FileNameSuffix, IPrettyPrint>();
      prettyPrints.Add(FileNameSuffix.A, new Word("A"));
      prettyPrints.Add(FileNameSuffix.B, new Word("B"));

      var fileStore = new FileStore(String.Empty);
      fileStore.WriteToFiles(prettyPrints, textWriterCreatorMock.Object);

      writer.Verify(x => x.Write(It.IsAny<string?>()),Times.Exactly(2));
    }
  }
}
