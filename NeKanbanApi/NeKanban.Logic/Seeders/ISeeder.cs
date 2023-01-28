namespace NeKanban.Logic.Seeders;

public interface ISeeder
{
    Task Run(CancellationToken ct);
}