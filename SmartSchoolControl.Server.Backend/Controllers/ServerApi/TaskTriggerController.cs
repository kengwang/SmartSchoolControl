using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartSchoolControl.Common.Base.Abstractions;
using SmartSchoolControl.Common.Base.ApiModels.Packages.Requests;
using SmartSchoolControl.Common.Base.DbModels;
using SmartSchoolControl.Server.Backend.Models;

namespace SmartSchoolControl.Server.Backend.Controllers.ServerApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskTriggerController : ControllerBase
    {
        private readonly IRepository<TaskTrigger,Guid> _taskTriggerRepository;

        public TaskTriggerController(IRepository<TaskTrigger, Guid> taskTriggerRepository)
        {
            _taskTriggerRepository = taskTriggerRepository;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ServerReturnBase<TaskTrigger>> Get(Guid id)
        {
            var trigger = await _taskTriggerRepository.FirstOrDefaultAsync(t=>t.Id == id);
            return trigger == null ? ServerReturnBase<TaskTrigger>.NotFound : new ServerReturnBase<TaskTrigger>(trigger);
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ServerReturnBase<TaskTrigger>> Add(ClientTaskTriggerAddPackage? package)
        {
            if (package is null) return ServerReturnBase<TaskTrigger>.ParamNotCompleted;
            var trigger = new TaskTrigger
            {
                Id = Guid.NewGuid(),
                Name = package.Name,
                Type = package.Type,
                TimesInDay = package.TimesInDay,
                DayInWeek = package.DayInWeek,
                Dates = package.Dates,
                DateTimes = package.DateTimes,
                ExecutionOnceTime = package.ExecutionOnceTime
            };
            return new ServerReturnBase<TaskTrigger>(await _taskTriggerRepository.InsertAsync(trigger));
        }

    }
}