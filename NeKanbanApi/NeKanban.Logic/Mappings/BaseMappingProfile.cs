using NeKanban.Common.Interfaces;

namespace NeKanban.Logic.Mappings;

public abstract class BaseMappingProfile<TSrc, TDest> : IMappingProfile<TSrc, TDest> where TSrc : class where TDest : class, IMapFrom<TSrc, TDest>
{
    public abstract Task<TDest> Map(TSrc source, CancellationToken ct);
    
    public virtual async Task<List<TDest>> MapCollection(List<TSrc> source, CancellationToken ct)
    {
        var list = new List<TDest>();
        foreach (var item in source)
        {
            list.Add(await Map(item, ct));
        }

        return list;
    }
}