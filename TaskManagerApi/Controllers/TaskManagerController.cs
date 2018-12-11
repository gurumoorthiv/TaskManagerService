using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using TaskManagerBusinessLayer;
using TaskManagerEntitiesModel;

using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace TaskManagerApi.Controllers
{
    [EnableCors("*","*","*")]
    [RoutePrefix("Task")]
    public class TaskManagerController : ApiController
    {
       private ITaskService taskService;

        public TaskManagerController(ITaskService taskService)
        {
            this.taskService = taskService;
        }
      

        [HttpGet]
        [Route("GetAllParentTasks")]
        [Route("")]
        public ICollection<TaskModel> GetAllParentTasks()
        {
            var ParentTask = taskService.GetAllParentTasks();
            Console.WriteLine(ParentTask.ToString());
            return ParentTask;
        }

        [HttpGet]
        [Route("GetAllTasks")]
        public ICollection<TaskModel> GetAllTasks()
        {
            var AllTask = taskService.GetAllTasks();
            Console.WriteLine(AllTask.ToString());
            return AllTask;
        }

        // GET: api/Task/5
        [ResponseType(typeof(TaskModel))]
        [Route("GetTaskById/{id:int}")]
        [HttpGet]
        public IHttpActionResult GetTaskById(int id)

        {
            TaskModel taskModel = taskService.GetTaskById(id);
            if (taskModel == null)
            {
                return NotFound();
            }

            return Ok(taskModel);
        }

        [HttpPost]
        [Route("AddTask")]
        [ResponseType(typeof(TaskModel))]
        [EnableCors("*", "*", "*")]
        public IHttpActionResult AddTask(TaskModel task)
        {
            var tempTask = taskService.GetTaskById(task.TaskId);
            if (tempTask != null)
            {
                return BadRequest();
            }
            var taskModel = taskService.AddTask(task);
            Console.WriteLine(taskModel.ToString());
            return Ok(taskModel);
        }

        [HttpPut]
        [Route("UpdateTaks")]
        [ResponseType(typeof(TaskModel))]
        public IHttpActionResult UpdateTaks(TaskModel task)
        {
            var tempTask = taskService.GetTaskById(task.TaskId);
            if (tempTask == null)
            {
                return BadRequest();
            }
            var updateTaks = taskService.UpdateTaks(task);
            Console.WriteLine(updateTaks.ToString());
            return Ok(updateTaks);
        }
        
        [HttpPut]
        [Route("CloseTask")]
        [ResponseType(typeof(TaskModel))]
        public IHttpActionResult CloseTask(TaskModel task)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (taskService.GetTaskById(task.TaskId) ==null)
            {
                return BadRequest("You cannot update an closed task");
            }
            var CloseTask = taskService.CloseTask(task);
            Console.WriteLine(CloseTask.ToString());
            return Ok(CloseTask);
        }


        // GET: api/Task/5
        [ResponseType(typeof(TaskModel))]
        [Route("DeleteTaskById")]
        [HttpDelete]
        public IHttpActionResult DeleteTaskById(int taskid)
        {
            var task = taskService.GetTaskById(taskid);
            if (task == null)
            {
                return NotFound();
            }
            bool deleteTask =   taskService.DeleteTaks(task);
            if (!deleteTask)
            {
                return NotFound();
            }
            return Ok(task);
        }
    }
}