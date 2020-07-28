using System.IO;

namespace WordCounter.Store
{
  public class TextWriterCreator : ITextWriterCreator
  {
    public TextWriter Create(string path) => new StreamWriter(path);
  }
}