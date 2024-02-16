using API.Definitions.Repositories;
using API.Definitions.Services;
using API.Domain.Ejercicios;

namespace API.Application.Ejercicios
{
    public class EjercicioAppService : CrudAppService<Ejercicio, Guid>
    {
        public EjercicioAppService(IRepository<Ejercicio, Guid> ejercicioRepository) : base(ejercicioRepository) { }
    }
}
