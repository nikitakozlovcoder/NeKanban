namespace NeKanban.Common.Interfaces;

public interface IMappingProfile<TSrc, TDest> where TDest : class, IMapFrom<TSrc, TDest> where TSrc : class
{
    Task<TDest> Map(TSrc source, CancellationToken ct);
    Task<List<TDest>> MapCollection(List<TSrc> source, CancellationToken ct);
}