namespace NeKanban.Data;

public interface IHasPk<TPkType>
{
    public TPkType Id { get; set; }
}