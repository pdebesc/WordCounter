using WordCounter.Store;

namespace WordCounter.ArgumentParser
{
  interface IProgramArguments
  {
    /// <summary>
    /// Get data to be processed
    /// </summary>
    public IInputData GetInputData { get; }

    /// <summary>
    /// Store processed data.
    /// </summary>
    public IStore GetDataStore { get; }
  }
}   