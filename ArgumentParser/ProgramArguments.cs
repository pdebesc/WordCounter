using WordCounter.Store;

namespace WordCounter.ArgumentParser
{
  class ProgramArguments : IProgramArguments
  {
    public ProgramArguments(Options options)
    {
      GetInputData = new FileInputData(options.Input);
      GetDataStore = new FileStore(options.Input);
    }

    public IStore GetDataStore { get; }

    public IInputData GetInputData { get; }
  }
}