namespace NeKanban.Common.Entities;

public interface IHasPk<TPkType>
{
    public TPkType Id { get; set; }
}