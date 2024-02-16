using System.ComponentModel.DataAnnotations;

namespace API.Domain.Ejercicios
{
    public class Ejercicio
    {
        [Key]
        public Guid Id { get; set; }
        public string Title { get; set; }
    }
}
