using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Batteries.Repository;

public static class ChangeTrackerExtensions
{
    public static void AddSoftDelete(this ChangeTracker changeTracker)
    {
        changeTracker.DetectChanges();
        var entities =
            changeTracker
                .Entries()
                .Where(t => t is { Entity: ISoftDeletable, State: EntityState.Deleted });
        
        foreach(var entry in entities)
        {
            var entity = (ISoftDeletable)entry.Entity;
            entity.IsDeleted = true;
            entry.State = EntityState.Modified;
        }
    }
}