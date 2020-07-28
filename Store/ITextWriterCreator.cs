using System.IO;

namespace WordCounter.Store
{
  public interface ITextWriterCreator
  {
    TextWriter Create(string path);
  }
}