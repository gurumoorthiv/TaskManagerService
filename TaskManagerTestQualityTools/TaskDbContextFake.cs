using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using TaskManagerDataAcessLayer;
using TaskManagerEntitiesModel;
using NSubstitute;
namespace TaskManagerTestQualityTools
{
    public class TaskDbContextFake : ITaskDbContext
    {
        public bool ThrowErrorOnNextMethod { get; set; }
        public IDbSet<TaskModel> tasks;
        public IDbSet<TaskModel> Tasks {
            get
            {
                return tasks;
            }
            set
            {
                 tasks =value;
            }
        }
        public TaskDbContextFake()
        {
            IDbSet<TaskModel> task = NSubstitute.Substitute.For<IDbSet<TaskModel>, IQueryable<TaskModel>>();
            //tasks.Provider = TasksData.AllTaks.Provider;
            task.Provider.Returns(TaskServiceFakeData.TasksData.AllTaks.Provider);
            task.Expression.Returns(TaskServiceFakeData.TasksData.AllTaks.Expression);
            task.ElementType.Returns(TaskServiceFakeData.TasksData.AllTaks.ElementType);
            task.GetEnumerator().Returns(TaskServiceFakeData.TasksData.AllTaks.GetEnumerator());
            this.Tasks = task;

        }
            public void Dispose()
        {
            Tasks = null;
            return;
        }

        public int SaveChanges()
        {
            if (ThrowErrorOnNextMethod)
                throw new Exception("Error");
            return 1;
        }

        public void SetModifield(object value)
        {
            return;
        }
    }
}
