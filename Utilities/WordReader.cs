using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using static System.Char;

namespace WordCounter.Utilities
{
  static class WordReader
  {
    /// <summary>
    /// Extract words, that only contains letter, from a file. 
    /// </summary>
    /// <param name="filePath">The path to the file to be read</param>
    /// <returns>Returns all words in a file asynchronously</returns>
    public static async IAsyncEnumerable<string> GetWordsAsync(string filePath)
    {
      using var stream = File.OpenText(filePath);
      await foreach (var s in GetWordsAsync(stream))
        yield return s;
    }

    internal static async IAsyncEnumerable<string> GetWordsAsync(StreamReader stream)
    {
      using (stream)
      {
        var sb = new StringBuilder();

        var bufferLength = 100;
        while (!stream.EndOfStream)
        {
          var charsBuffer = new char[bufferLength];
          await stream.ReadAsync(charsBuffer, 0, bufferLength);
          foreach (var currentChar in charsBuffer)
          {
            if (IsLetter(currentChar))
              sb.Append(currentChar);
            else if(IsDigit(currentChar))
              throw new ArgumentException($"Cannot handle files with digits. Digit {currentChar} are present.");
            else if (sb.Length > 0)
            {
              var word = sb.ToString();
              sb = new StringBuilder();
              yield return word;
            }
          }
        }
      }
    }
  }
}
