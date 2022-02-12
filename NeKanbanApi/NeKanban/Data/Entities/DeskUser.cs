﻿namespace NeKanban.Data.Entities;

public class DeskUser : IHasPk<int>
{
    public int Id { get; set; }
    public int DeskId { get; set; }
    public int UserId { get; set; }
    public virtual ApplicationUser? User { get; set; }
    public virtual Desk? Desk { get; set; }
    
}