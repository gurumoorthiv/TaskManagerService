using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerEntitiesModel;

namespace TaskManagerDataAcessLayer
{
    public class TaskDbContext : DbContext, ITaskDbContext
    {
        private IDbSet<TaskModel> tasks;

        public TaskDbContext() : base("name=taskManagerDbSource")
        {
            Tasks = this.Set<TaskModel>();
        }
        public IDbSet<TaskModel> Tasks
        {
            get { return tasks; }
            set { tasks = value; }
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var taskMap = modelBuilder.Entity<TaskModel>();
            taskMap.HasKey(x => x.TaskId);
            taskMap.Property(x => x.TaskId).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity).HasColumnName("Task_Id");
            taskMap.Property(x => x.TaskDescription).HasColumnName("TaskDescription");
            taskMap.Property(x => x.StartDate).HasColumnName("StartDate");
            taskMap.Property(x => x.EndDate).HasColumnName("EndDate");
            taskMap.Property(x => x.Priority).HasColumnName("Priority");
            taskMap.Property(x => x.IsClosed).HasColumnName("IsClosed");
            taskMap.HasOptional(x => x.ParentTask).WithMany(x => x.ChildTasks).HasForeignKey(x => x.ParentTaskId);
            taskMap.Property(x => x.ParentTaskId).IsOptional();
            taskMap.ToTable("Tasks");

        }

        //public int SaveChanges()
        //{
        //    //throw new NotImplementedException();
        //    return 0;
        //}

        public void SetModifield(object value)
        {
            Entry(value).State = EntityState.Modified;
        }

    }
}
