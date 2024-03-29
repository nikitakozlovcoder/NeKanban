﻿namespace NeKanban.Data.Infrastructure.QueryFilters;

public class QueryFilterSettingsScope : IDisposable
{
    public required QueryFilterSettings Holder { get; set; }
    public required QueryFilterSettingsDefinitions PrevSettingsDefinitions { get; set; }

    public void Dispose()
    {
        Holder.SettingsDefinitions = PrevSettingsDefinitions;
    }
}