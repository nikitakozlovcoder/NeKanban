namespace Batteries.Repository;

public interface IHasPk<TPkType>
{
    public TPkType Id { get; set; }
}