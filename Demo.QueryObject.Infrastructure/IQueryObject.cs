using System.Linq.Expressions;

namespace Demo.QueryObject.Infrastructure
{
    public interface IQueryObject<TEntity> where TEntity : class, new()
    {
        Task<IEnumerable<TEntity>> ExecuteAsync();
        QueryObject<TEntity> Filter(Expression<Func<TEntity, bool>> predicate);
        QueryObject<TEntity> OrderBy<TKey>(Expression<Func<TEntity, TKey>> selector, bool ascending = true);
        QueryObject<TEntity> Page(int page, int pageSize);
    }
}