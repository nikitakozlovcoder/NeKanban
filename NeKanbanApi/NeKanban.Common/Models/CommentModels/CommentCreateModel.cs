using System.ComponentModel.DataAnnotations;

namespace NeKanban.Common.Models.CommentModels;

public class CommentCreateModel
{
    [MinLength(10)]
    public string Body { get; set; } = null!;
}