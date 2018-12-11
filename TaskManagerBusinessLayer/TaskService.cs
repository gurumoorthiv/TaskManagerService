using System;
using System.Collections.Generic;
using TaskManagerEntitiesModel;
using TaskManagerDataAcessLayer;
using System.Data.Entity;
using System.Linq;

namespace TaskManagerBusinessLayer
{
    public class TaskService : ITaskService
    {
        public ITaskDbContext taskDbContext;

        public TaskService(ITaskDbContext _taskDbContext)
        {
            taskDbContext = _taskDbContext;
        }

        public TaskModel AddTask(TaskModel task)
        {
            taskDbContext.Tasks.Add(task);
            if (taskDbContext.SaveChanges() >= 0)
            {
                return task;
            }
            return null;
        }

        public TaskModel UpdateTaks(TaskModel task)
        {
            if (task.IsClosed)
            {
                throw new Exception("You cannot update an closed task");
            }
            taskDbContext.Tasks.Attach(task);
            taskDbContext.SetModifield(task);
            if(taskDbContext.SaveChanges() >0)
            {
                return task;
            }
            return null;
        }

        public TaskModel CloseTask(TaskModel task)
        {
            var selectedTask = GetTaskById(task.TaskId);
            if (selectedTask == null)
                throw new Exception("You cannot update an closed task");
            else
                selectedTask.IsClosed = true;
            taskDbContext.Tasks.Attach(selectedTask);
            taskDbContext.SetModifield(selectedTask);
            if (taskDbContext.SaveChanges() > 0)
            {
                return selectedTask;
            }
            return null;
        }

        public bool DeleteTaks(TaskModel task)
        {
            taskDbContext.Tasks.Remove(task);
            return taskDbContext.SaveChanges() == 1;
        }

        public void Dispose()
        {
            taskDbContext.Dispose();
        }

        public ICollection<TaskModel> GetAllParentTasks()
        {
            //var taskCollectionList = taskDbContext.Tasks.ToListAsync<TaskModel>().Result;
            return taskDbContext.Tasks != null && taskDbContext.Tasks.Count() > 0 ? taskDbContext.Tasks.ToList().Where(x => x.ChildTasks != null && x.ChildTasks.Count() > 0).ToList() : taskDbContext.Tasks.ToList();  
            //return taskDbContext.Tasks.ToList();
        }

        public ICollection<TaskModel> GetAllTasks()
        {
            return taskDbContext.Tasks!= null && taskDbContext.Tasks.Count() >0 ? taskDbContext.Tasks.Include(x => x.ParentTask).Include(x => x.ChildTasks).ToList() : taskDbContext.Tasks.ToList();
        }
        public ICollection<TaskModel> QueryTask(TaskManagerQueryModel query)
        {
            throw new NotImplementedException();
        }

        public TaskModel GetTaskById(int taskid)
        {
            return taskDbContext.Tasks.Include(x=>x.ParentTask).FirstOrDefault(x => x.TaskId == taskid);
        }
    }
}
