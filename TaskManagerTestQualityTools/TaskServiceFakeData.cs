using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerEntitiesModel;

namespace TaskManagerTestQualityTools
{
    public static class TaskServiceFakeData
    {
        public static class TasksData
        {
            public static TaskModel Task1 = new TaskModel()
            {
                TaskId = 1,
                TaskDescription = "Parent Task",
                StartDate = new DateTime(2018, 09, 05),
                EndDate = new DateTime(2018, 09, 05),
                Priority = 1,
                ParentTask = null,
                ChildTasks = new List<TaskModel>() { Task2, Task3 }
            };
            public static TaskModel Task2 = new TaskModel()
            {
                TaskId = 2,
                TaskDescription = "Second Task",
                StartDate = new DateTime(2018, 09, 05),
                EndDate = new DateTime(2018, 09, 05),
                Priority = 1,
                ParentTask = Task1
            };
            public static TaskModel Task3 = new TaskModel()
            {
                TaskId = 3,
                TaskDescription = "Second Task",
                StartDate = new DateTime(2018, 09, 05),
                EndDate = new DateTime(2018, 09, 05),
                Priority = 1,
                ParentTask = Task1
            };

            public static IQueryable<TaskModel> AllTaks = new List<TaskModel>() { Task1, Task2, Task3 }.AsQueryable();
            public static IQueryable<TaskModel> AllParentTaks = new List<TaskModel>() { Task1, Task2, Task3 }.AsQueryable().Where(x => x.ChildTasks != null && x.ChildTasks.Count() > 0);

        }
    }
}
