using Microsoft.AspNetCore.Mvc;
using Server.Backend.Models.Packages.Requests.ClientRequests;
using Server.Backend.Models.Packages.Returns;
using Server.Base.Abstractions;
using Server.Base.DbModels;

namespace Server.Backend.Controllers.ClientApi
{
    [Route("api/ClientApi/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IRepository<Client,Guid> _clientRepository;

        public ClientController(IRepository<Client,Guid> clientRepository)
        {
            _clientRepository = clientRepository;
        }

        [Route("[action]")]
        [HttpPost]
        public ObjectResult Register([FromBody]ClientRegister? registerPackage)
        {
            if (registerPackage is null) return ServerReturnBase.ParamNotCompleted;
            var ipAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
            _clientRepository.Insert(new Client
            {
                Id = registerPackage.Id,
                Name = registerPackage.Name ?? "未命名",
                IP = ipAddress,
                FriendlyName = registerPackage.FriendlyName ?? "未命名",
                LastHeartBeatTime = DateTime.Now
            });
            return ServerReturnBase.Ok;
        }
    }
}