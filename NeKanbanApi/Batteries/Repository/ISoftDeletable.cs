namespace Batteries.Repository;

public interface ISoftDeletable
{
    public bool IsDeleted { get; set; }
}