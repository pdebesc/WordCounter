using System.IO;
using System.Linq;

using NUnit.Framework;

using WordCounter.ArgumentParser;

using static WordCounter.Tests.TestConstants;


namespace WordCounter.Tests.ArgumentParser
{
  [TestFixture]
  class FileInputDataTest
  {
    [Test]
    public void GetInputDataTest_CanLocateTxtFiles()
    {
      var currentTestDirectory = TestContext.CurrentContext.TestDirectory;
      var testFilesDirectoryPath = Path.Combine(
        currentTestDirectory,
        TestsDirectoryName,
        ArgumentParserDirectoryName,
        TestFilesDirectoryName);

      var fileInputData = new FileInputData(testFilesDirectoryPath);

      var filePaths = fileInputData.GetInputData.ToList();
      Assert.That(
        filePaths.Count(),
        Is.EqualTo(2),
        () => $"Found {filePaths.Count} files at {testFilesDirectoryPath}");
      Assert.That(filePaths.All(path => path.EndsWith(".txt")));
    }
  }
}
