using API.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace API.Definitions.Repositories
{
    public class Repository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : class
    {
        private readonly FitnessDbContext _context;

        public Repository(FitnessDbContext context)
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

        public async Task<TEntity> UpdateAsync(TKey id, TEntity entity)
        {
            if (!await EntityExists(id))
            {
                return null;
            }

            _context.Entry(entity).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Handle concurrency exception if needed
                throw;
            }

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

        public async Task<bool> DeleteAsync(TKey id)
        {
            var entity = await _context.Set<TEntity>().FindAsync(id);
            if (entity == null)
            {
                return false;
            }

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
