using System.ComponentModel.DataAnnotations;

namespace NeKanban.Common.Models.CommentModels;

public class CommentCreateModel
{
    [MinLength(10)]
    public required string Body { get; set; }
}