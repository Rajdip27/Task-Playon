namespace TaskPlayon.Application.Extensions;

public static class EnumerableExtensions
{
    /// <summary>
    /// Generic extension to map a list of IDs into entities with Id.
    /// </summary>
    public static List<TEntity> ToEntities<TEntity>(
        this IEnumerable<long> ids,
        long Id,
        Func<long, long, TEntity> mapFunc)
    {
        return ids.Select(id => mapFunc(id, Id)).ToList();
    }
}
