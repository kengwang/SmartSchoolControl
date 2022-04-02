using Microsoft.AspNetCore.Mvc;
using Server.Backend.Models.Packages.Returns;
using Server.Base.Abstractions;
using Server.Base.DbModels;

namespace Server.Backend.Controllers.ServerApi
{
    [Route("api/ServerApi/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private IRepository<Client,Guid> _clientRepository;
        private ILogger<ClientController> _logger;

        public ClientController(IRepository<Client,Guid> clientRepository, ILogger<ClientController> logger)
        {
            _clientRepository = clientRepository;
            _logger = logger;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ObjectResult> Get(Guid id)
        {
            var client = await _clientRepository.FirstOrDefaultAsync(t => t.Id == id);
            return client == null ? ServerReturnBase.NotFound : new ServerReturnBase(client);
        }
    }
}