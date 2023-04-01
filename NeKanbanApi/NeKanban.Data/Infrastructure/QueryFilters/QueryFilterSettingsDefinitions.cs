namespace NeKanban.Data.Infrastructure.QueryFilters;

public class QueryFilterSettingsDefinitions
{
    public bool ToDoDraftFilter { get; init; } = true;
    public bool CommentDraftFilter { get; init; } = true;
    public bool DeskUserDeletedFilter { get; set; } = true;
}