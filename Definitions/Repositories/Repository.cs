using API.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace API.Definitions.Repositories
{
    public class Repository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : class
    {
        private readonly EntityFrameworkCore.AppDbContext _context;

        public Repository(EntityFrameworkCore.AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<TEntity>> GetListAsync()
        {
            return await _context.Set<TEntity>().ToListAsync();
        }

        public async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> condition)
        {
            return await _context.Set<TEntity>().FirstOrDefaultAsync(condition);
        }

        public async Task<TEntity> InsertAsync(TEntity entity)
        {
            _context.Set<TEntity>().Add(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task DeleteManyAsync(IEnumerable<TEntity> entities)
        {
            if (entities == null || !entities.Any())
            {
                return;
            }

            _context.Set<TEntity>().RemoveRange(entities);
            await _context.SaveChangesAsync();
        }

        public async Task InsertManyAsync(IEnumerable<TEntity> entities)
        {
            if (entities == null || !entities.Any())
            {
                return;
            }

            _context.Set<TEntity>().AddRange(entities);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
            
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<IQueryable<TEntity>> GetQueryableAsync()
        {
            var entities = await _context.Set<TEntity>().ToListAsync();
            return entities.AsQueryable();
        }

        private async Task<bool> EntityExists(TKey id)
        {
            return await _context.Set<TEntity>().AnyAsync(e => EqualityComparer<TKey>.Default.Equals((TKey)e.GetType().GetProperty("Id").GetValue(e), id));
        }
    }
}
