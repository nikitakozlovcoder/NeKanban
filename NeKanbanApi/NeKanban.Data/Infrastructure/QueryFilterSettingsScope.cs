namespace NeKanban.Data.Infrastructure;

public class QueryFilterSettingsScope : IDisposable
{
    public required QueryFilterSettings Holder { get; set; }
    public required QueryFilterSettingsDefinitions PrevSettingsDefinitions { get; set; }

    public void Dispose()
    {
        Holder.SettingsDefinitions = PrevSettingsDefinitions;
    }
}