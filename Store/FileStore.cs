using System;
using System.Collections.Generic;
using System.IO;

using WordCounter.Words;

namespace WordCounter.Store
{
  class FileStore : IStore
  {
    readonly string outPath;

    public FileStore(string outPath)
    {
      this.outPath = outPath;
    }

    public void Store<TKey, TValue>(IReadOnlyDictionary<TKey, TValue> keyValuePairs)
      where TKey : Enum
      where TValue : notnull, IPrettyPrint
    {
      CreateOrRecreateOutputDirectory();
      WriteToFiles(keyValuePairs, new TextWriterCreator());
    }

    internal void WriteToFiles<TKey, TValue>(IReadOnlyDictionary<TKey, TValue> keyValuePairs, ITextWriterCreator writerCreator)
      where TKey : Enum 
      where TValue : notnull, IPrettyPrint
    {
      foreach (var kvp in keyValuePairs)
      {
        var fileNameSuffix = Enum.GetName(typeof(TKey), kvp.Key);
        var fullFilePath = CreateFullFilePath(StoreLocation, fileNameSuffix);
        using var stream = writerCreator.Create(fullFilePath);
        stream.Write(kvp.Value.PrettyPrint);
      }
    }

    void CreateOrRecreateOutputDirectory()
    {
      if (Directory.Exists(StoreLocation))
        Directory.Delete(StoreLocation, true);
      Directory.CreateDirectory(StoreLocation);
    }

    public string StoreLocation => Path.Combine(outPath, "OutFiles");

    static string CreateFullFilePath(string directoryPath, string fileNameSuffix)
    {
      var fileName = CreateFileName(fileNameSuffix);
      return Path.Combine(directoryPath, fileName);
    }

    internal static string CreateFileName(string fileNameSuffix)
    {
      var filePrefix = "FILE";
      var txtFileExtension = ".txt";
      return $"{filePrefix}_{fileNameSuffix}{txtFileExtension}";
    }
  }
}