using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerEntitiesModel;
using TaskManagerDataAcessLayer;
namespace TaskManagerBusinessLayer
{
    public interface ITaskService : IDisposable
    {
        TaskModel GetTaskById(int taskid);
        TaskModel UpdateTaks(TaskModel id);

        TaskModel AddTask(TaskModel task);

        bool DeleteTaks(TaskModel task);

        ICollection<TaskModel> GetAllTasks();

        ICollection<TaskModel> QueryTask(TaskManagerQueryModel query);

        ICollection<TaskModel> GetAllParentTasks();

        TaskModel CloseTask(TaskModel task);
    }
}
