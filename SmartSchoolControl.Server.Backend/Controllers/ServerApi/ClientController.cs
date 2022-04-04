using Microsoft.AspNetCore.Mvc;
using SmartSchoolControl.Common.Base.Abstractions;
using SmartSchoolControl.Common.Base.DbModels;
using SmartSchoolControl.Server.Backend.Models;

namespace SmartSchoolControl.Server.Backend.Controllers.ServerApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IRepository<Client,Guid> _clientRepository;
        private ILogger<ClientController> _logger;

        public ClientController(IRepository<Client,Guid> clientRepository, ILogger<ClientController> logger)
        {
            _clientRepository = clientRepository;
            _logger = logger;
        }
        
        [HttpPost]
        public async Task<ServerReturnBase<Client>> Get(Guid id)
        {
            var client = await _clientRepository.FirstOrDefaultAsync(t => t.Id == id);
            return client == null ? ServerReturnBase<Client>.NotFound : new ServerReturnBase<Client>(client);
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ServerReturnBase<List<Client>>> GetAll()
        {
            return new ServerReturnBase<List<Client>>(await _clientRepository.GetAllListAsync());
        }
    }
}