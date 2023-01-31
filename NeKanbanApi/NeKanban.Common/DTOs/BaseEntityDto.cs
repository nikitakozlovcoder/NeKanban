namespace NeKanban.Common.DTOs;

public class BaseEntityDto<T>
{
    public T Id { get; set; } = default!;
}