using Microsoft.EntityFrameworkCore;

namespace Demo.QueryObject.Infrastructure.EFCore
{
    public class EFCoreQueryObject<TEntity> : QueryObject<TEntity> where TEntity : class, new()
    {
        private DbContext _dbContext;

        public EFCoreQueryObject(DbContext dbContext)
        {
            _dbContext = dbContext;
            _query = _dbContext.Set<TEntity>().AsQueryable();
        }

        public override async Task<IEnumerable<TEntity>> ExecuteAsync()
        {
            return await _query.ToListAsync();
        }
    }
}