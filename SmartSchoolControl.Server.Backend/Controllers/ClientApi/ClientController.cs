using Microsoft.AspNetCore.Mvc;
using SmartSchoolControl.Common.Base.Abstractions;
using SmartSchoolControl.Common.Base.ApiModels.Packages.Requests;
using SmartSchoolControl.Common.Base.ApiModels.Packages.Returns;
using SmartSchoolControl.Common.Base.DbModels;
using SmartSchoolControl.Server.Backend.Models;

namespace SmartSchoolControl.Server.Backend.Controllers.ClientApi
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IRepository<Client, Guid> _clientRepository;
        private readonly IPluginManager _pluginManager;

        public ClientController(IRepository<Client, Guid> clientRepository, IPluginManager pluginManager)
        {
            _clientRepository = clientRepository;
            _pluginManager = pluginManager;
        }

        [HttpPost]
        public async Task<ServerReturnBase> Register([FromBody] ClientRegisterPackage? registerPackage)
        {
            if (registerPackage is null) return ServerReturnBase.ParamNotCompleted;
            await _clientRepository.InsertAsync(new Client
            {
                Id = registerPackage.ClientId,
                Name = registerPackage.Name ?? "未命名",
                FriendlyName = registerPackage.FriendlyName ?? "未命名",
                LastHeartBeatTime = DateTime.Now
            });
            return ServerReturnBase.Ok;
        }

        [HttpPost]
        public async Task<ServerReturnBase<ServerOnlinePackage>> Online([FromBody] ClientOnlinePackage? onlinePackage)
        {
            if (onlinePackage is null) return ServerReturnBase<ServerOnlinePackage>.ParamNotCompleted;
            var client = await _clientRepository.FirstOrDefaultAsync(t => t.Id == onlinePackage.ClientId);
            return client is null
                ? new ServerReturnBase<ServerOnlinePackage>(false, "客户端不存在", 404, -404)
                : new ServerReturnBase<ServerOnlinePackage>(
                    new ServerOnlinePackage(client.Tasks, client.Permissions, _pluginManager.Plugins));
        }
    }
}