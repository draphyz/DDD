namespace DDD.Core.Domain
{
    public enum EntityState
    {
        Unchanged = 0, // Default
        Added,
        Modified,
        Deleted
    }
}