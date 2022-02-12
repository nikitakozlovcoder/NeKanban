namespace NeKanban.Data.Entities;

public class Column : IHasPk<int>
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
}