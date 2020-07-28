namespace WordCounter.Words
{
  interface IWordManager : IPrettyPrint
  {
    void Add(IWord word);
  }
}