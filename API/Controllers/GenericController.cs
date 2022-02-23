using DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GenericController<T> : ControllerBase where T : class
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly ILogger<T> _logger;

        public GenericController(IUnitOfWork unitOfWork, ILogger<T> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
    }
}
