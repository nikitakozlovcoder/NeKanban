namespace NeKanban.Data.Infrastructure;

public interface IHasPk<TPkType>
{
    public TPkType Id { get; set; }
}