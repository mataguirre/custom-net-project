using API.Definitions.Repositories;
using API.Domain.Ejercicios;

namespace API.Domain
{
    public class FitnessDataSeedContributor
    {
        private readonly IRepository<Ejercicio, Guid> _ejercicioRepository;
        public FitnessDataSeedContributor(IRepository<Ejercicio, Guid> ejercicioRepository)
        {
            _ejercicioRepository = ejercicioRepository;
        }

        public async Task SeedAsync()
        {
            /* Seed Tasks */
            await CreateEjercicios();
        }

        public async Task CreateEjercicios()
        {

            var listadoEjercicios = new List<Ejercicio>
            {
                new Ejercicio
                {
                    Title = "Press Banco Plano"
                },
                new Ejercicio
                {
                    Title = "Peso Muerto Rumano"
                },
                new Ejercicio
                {
                    Title = "Press Militar"
                },
                new Ejercicio
                {
                    Title = "Sentadillas Búlgaras"
                }
            };

            Console.WriteLine("Creando ejercicios de prueba");

            foreach (var ejercicio in listadoEjercicios)
            {
                var ejercicioExistente = await _ejercicioRepository.FirstOrDefaultAsync(x => x.Title.ToLower() == ejercicio.Title.ToLower());

                if(ejercicioExistente is null)
                {
                    await _ejercicioRepository.InsertAsync(ejercicio);
                }
            } 
        }
    }
}
