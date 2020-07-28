using System.Collections.Generic;

using WordCounter.Words;

namespace WordCounter.Store
{
  interface IStore
  {
    void Store<TKey, TValue>(IReadOnlyDictionary<TKey, TValue> keyValuePairs)
      where TKey : System.Enum
      where TValue : notnull, IPrettyPrint;

    string StoreLocation { get; }
  }
}