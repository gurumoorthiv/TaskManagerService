using TaskManagerEntitiesModel;
namespace TaskManagerApi.Tests.Utils
{
    public static class CloningHelper
    {
        public static TaskModel Clone(this TaskModel source)
        {
            var result = new TaskModel();
            result.TaskId = source.TaskId;
            if (source.ParentTask != null)
            {
                result.ParentTask = source.ParentTask.Clone();
            }
            result.ParentTaskId = source.ParentTaskId;
            result.Priority = source.Priority;
            result.StartDate = source.StartDate;
            result.EndDate = source.EndDate;
            result.TaskDescription = source.TaskDescription;
            result.IsClosed = source.IsClosed;
            return result;
        }
    }
}
