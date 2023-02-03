namespace NeKanban.Common.DTOs;

public class BaseEntityDto<T>
{
    public required T Id { get; set; }
}