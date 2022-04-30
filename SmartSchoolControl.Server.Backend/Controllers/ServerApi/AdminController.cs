using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartSchoolControl.Common.Base.ApiModels.Packages.Requests;
using SmartSchoolControl.Server.Backend.Models;

namespace SmartSchoolControl.Server.Backend.Controllers.ServerApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AdminController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [Route("[action]")]
        [HttpPost]
        public ServerReturnBase Login([FromBody] ClientLoginPackage? package)
        {
            if (package == null) return ServerReturnBase.ParamNotCompleted;
            if (package.Username == _configuration.GetValue("AdminName", "admin") &&
                package.Password == _configuration.GetValue("AdminPassword", "admin"))
            {
                return ServerReturnBase.Ok;
            }
            else
            {
                return new ServerReturnBase(false, "用户名或密码错误", 401, -401);
            }
        }
    }
}