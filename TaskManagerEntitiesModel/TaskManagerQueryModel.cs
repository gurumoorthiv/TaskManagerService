using System;

namespace TaskManagerEntitiesModel
{
    [Serializable]
    public class TaskManagerQueryModel
    {
        public string TaskName { get; set; }
        public int? PriorityFrom { get; set; }
        public int? PriorityTo { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string ParentTask { get; set; }
    }
}
