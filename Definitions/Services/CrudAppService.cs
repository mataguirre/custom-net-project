using API.Definitions.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Definitions.Services
{
    [ApiController]
    [Route("api/[controller]")]
    public class CrudAppService<TEntity, TKey> : ControllerBase
        where TEntity : class
    where TKey : notnull
    {
        private readonly IRepository<TEntity, TKey> _repository;

        public CrudAppService(IRepository<TEntity, TKey> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TEntity>>> GetListAsync()
        {
            var entities = await _repository.GetListAsync();
            return Ok(entities);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TEntity>> GetAsync(TKey id)
        {
            var entity = await _repository.FirstOrDefaultAsync(e => EF.Property<TKey>(e, "Id").Equals(id));

            if (entity == null)
            {
                return NotFound();
            }

            return entity;
        }

        [HttpPost]
        public async Task<ActionResult<TEntity>> CreateAsync(TEntity entity)
        {
            var createdEntity = await _repository.InsertAsync(entity);
            return CreatedAtAction(nameof(GetAsync), new { id = createdEntity.GetType().GetProperty("Id").GetValue(createdEntity) }, createdEntity);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(TKey id, TEntity entity)
        {
            var updatedEntity = await _repository.UpdateAsync(id, entity);

            if (updatedEntity == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(TKey id)
        {
            var deleted = await _repository.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
