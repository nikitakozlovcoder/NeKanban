using Batteries.Injection.Attributes;
using JetBrains.Annotations;

namespace NeKanban.Data.Infrastructure;

[Injectable]
[UsedImplicitly]
public class QueryFilterSettings
{
    public QueryFilterSettingsDefinitions SettingsDefinitions { get; set; } = new ();
    public IDisposable CreateScope(QueryFilterSettingsDefinitions settingsDefinitions)
    {
        var scope = new QueryFilterSettingsScope
        {
            PrevSettingsDefinitions = SettingsDefinitions,
            Holder = this
        };
        
        SettingsDefinitions = settingsDefinitions;
        return scope;
    }
}