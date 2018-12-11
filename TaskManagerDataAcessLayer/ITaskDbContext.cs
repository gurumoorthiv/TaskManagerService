using System;
using System.Data.Entity;
using TaskManagerEntitiesModel;

namespace TaskManagerDataAcessLayer
{
    public interface ITaskDbContext : IDisposable
    {
        IDbSet<TaskModel> Tasks { get; set; }
        int SaveChanges();

        void SetModifield(object value);
    }
}
