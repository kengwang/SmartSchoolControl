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
using SmartSchoolControl.Server.Db.Repositories;

namespace SmartSchoolControl.Server.Backend.Controllers.ServerApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly IRepository<ScheduledTask, Guid> _taskRepository;
        private readonly IRepository<TaskTrigger, Guid> _taskTriggerRepository;
        private readonly IRepository<Workflow, Guid> _workflowRepository;

        public TaskController(IRepository<ScheduledTask, Guid> taskRepository,
            IRepository<TaskTrigger, Guid> taskTriggerRepository, IRepository<Workflow, Guid> workflowRepository)
        {
            _taskRepository = taskRepository;
            _taskTriggerRepository = taskTriggerRepository;
            _workflowRepository = workflowRepository;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ServerReturnBase<List<ScheduledTask>>> GetAll()
        {
            return new ServerReturnBase<List<ScheduledTask>>(await _taskRepository.GetAllListAsync());
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ServerReturnBase<ScheduledTask>> Get(Guid id)
        {
            var task = await _taskRepository.FirstOrDefaultAsync(t => t.Id == id);
            return task == null ? new ServerReturnBase<ScheduledTask>(false, "任务不存在", 404, -404) : new ServerReturnBase<ScheduledTask>(task);
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ServerReturnBase> Add(ClientTaskAddPackage? taskAddPackage)
        {
            if (taskAddPackage == null) return ServerReturnBase.ParamNotCompleted;
            var triggers = taskAddPackage.TriggerIds.Select(t => _taskTriggerRepository.FirstOrDefault(tt => tt.Id == t)).ToList();
            if (triggers.Any(t => t == null)) return new ServerReturnBase(false, "触发器不存在", 404, -404);
            var workflow = await _workflowRepository.FirstOrDefaultAsync(w => w.Id == taskAddPackage.WorkflowId);
            if (workflow == null) return new ServerReturnBase(false, "工作流不存在", 404, -404);
            await _taskRepository.InsertAsync(new ScheduledTask
            {
                Id = Guid.NewGuid(),
                Name = taskAddPackage.Name,
                Description = taskAddPackage.Description,
                Enabled = taskAddPackage.Enabled,
                Triggers = triggers!,
                Workflow = workflow
            });
            return ServerReturnBase.Ok;
        }
    }
}