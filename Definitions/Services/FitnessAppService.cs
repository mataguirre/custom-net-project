using Microsoft.AspNetCore.Mvc;

namespace API.Definitions.Services
{
    [ApiController]
    [Route("api/[controller]")]
    public class FitnessAppService<TEntity> : ControllerBase
    {
    }
}
