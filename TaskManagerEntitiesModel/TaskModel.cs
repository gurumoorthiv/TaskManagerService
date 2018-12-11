using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace TaskManagerEntitiesModel
{
    [Serializable]
    public class TaskModel//:IValidatableObject
    {

        private int taskId;
        [Required]
        public int TaskId
        {
            get { return taskId; }
            set { taskId = value; }
        }

        private TaskModel parentTask;

        public TaskModel ParentTask
        {
            get { return parentTask; }
            set { parentTask = value; }
        }

        private ICollection<TaskModel> childTasks;

        public ICollection<TaskModel> ChildTasks
        {
            get { return childTasks; }
            set { childTasks = value; }
        }

        private int? parentTaskId;
        
        public int? ParentTaskId
        {
            get { return parentTaskId; }
            set { parentTaskId = value; }
        }

        private string taskDescription;

        [Required]
        public string TaskDescription
        {
            get { return taskDescription; }
            set { taskDescription = value; }
        }


        private DateTime? startDate;

        [Required]
        public DateTime? StartDate
        {
            get { return startDate; }
            set { startDate = value; }
        }

        private DateTime? endDate;
         

        public DateTime? EndDate
        {
            get { return endDate; }
            set { endDate = value; }
        }

        private int priority;

        [Range(1, 30)]
        public int Priority
        {
            get { return priority; }
            set { priority = value; }
        }

        private bool isClosed;

        public bool IsClosed
        {
            get { return isClosed; }
            set { isClosed = value; }
        }
    }
}
