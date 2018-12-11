using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Web.Http.Results;
using TaskManagerApi.Controllers;
using TaskManagerApi.Tests.Utils;
using TaskManagerBusinessLayer;
using TaskManagerDataAcessLayer;
using TaskManagerEntitiesModel;
using TaskManagerTestQualityTools;
using Unity;

namespace TaskManagerApi.Tests
{
    [TestFixture]
    public class TaskControllerTest
    {
        private TaskManagerController Controller;
        private UnityContainer container;
        private ITaskService taskService;
        [SetUp]
        public void Setup()
        {
            container = new UnityContainer();
            container.RegisterType<ITaskDbContext, TaskDbContextFake>();
            container.RegisterType<ITaskService, TaskService>();
            taskService = container.Resolve<ITaskService>();
            Controller = new TaskManagerController(taskService);
        }

        [TestCase]
        public void When_SearchForAllTask_Then_AllTasksReceived()
        {
            // Arrange & Act
            var result = Controller.GetAllTasks();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count());
        }

        [TestCase]
        public void When_SearchForAllParentTask_Then_AllParentTasksReceived()
        {
            // Arrange & Act
            var result = Controller.GetAllParentTasks();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
        }

        [TestCase]
        public void When_SearchByTaskIdAndAvailable_Then_OkReceived()
        {
            // Arrange & Act
            var result = Controller.GetTaskById(1);
            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkNegotiatedContentResult<TaskModel>>(result);
            var castedResult = result as OkNegotiatedContentResult<TaskModel>;
            Assert.AreEqual(1, castedResult.Content.TaskId);
        }

        [TestCase]
        public void When_SearchByTaskIdAndNotAvailable_Then_NotFoundReceived()
        {
            // Arrange & Act
            var result = Controller.GetTaskById(101);
            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [TestCase]
        public void When_DeleteExistingTaskModel_Then_Deleted()
        {
            // Arrange & Act
            var result = Controller.DeleteTaskById(1);
            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkNegotiatedContentResult<TaskModel>>(result);
            var castedResult = result as OkNegotiatedContentResult<TaskModel>;
            Assert.AreEqual(1, castedResult.Content.TaskId);
        }
        [TestCase]
        public void When_DeleteNonExistingTaskModel_Then_Error()
        {
            // Arrange & Act
            var result = Controller.DeleteTaskById(101);
            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<NotFoundResult>(result);

        }

        [TestCase]
        public void When_DeleteTaskAndDBError_Then_Fail()
        {
            // Arrange
            var task = TaskServiceFakeData.TasksData.Task1;
            task.IsClosed = false;
            var service = Substitute.For<ITaskService>();
            service.GetTaskById(1).Returns(x => { return task; });
            service.DeleteTaks(task).Returns(x => { return false; });
            Controller = new TaskManagerController(service);

            // Arrange 
            var result = Controller.DeleteTaskById(task.TaskId);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [TestCase]
        public void When_Put_Task_WhichIsNotAvailale_Then_Error()
        {
            // Arrange & Act
            TaskModel task = TaskServiceFakeData.TasksData.Task1.Clone();
            task.TaskId = 111;
            var result = Controller.UpdateTaks(task);
            
            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [TestCase]
        public void When_Put_AnExistingTask_Then_Pass()
        {
            // Arrange & Act
            var task = TaskServiceFakeData.TasksData.Task1;
            task.IsClosed = false;
            var result = Controller.UpdateTaks(task);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkNegotiatedContentResult<TaskModel>>(result);
        }

        [TestCase]
        public void When_PutTaskWithInvalidModelState_Then_Should_Fail()
        {
           // Arrange
           var task = TaskServiceFakeData.TasksData.Task1.Clone();
            task.TaskDescription = null;
            task.Priority = 35;

            //Act
            Controller.ModelState.AddModelError("Priorty", "Priority is invalid");
            var result = Controller.UpdateTaks(task);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkNegotiatedContentResult<TaskModel>>(result);

        }

        [TestCase]
        public void When_Post_AnExistingTask_Then_Fail()
        {
           // Arrange & Act
            var task = TaskServiceFakeData.TasksData.Task1.Clone();
            task.IsClosed = false;
            var result = Controller.AddTask(task);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [TestCase]
        public void When_Post_AnNewTask_Then_Pass()
        {
           // Arrange
           var task = new TaskModel { EndDate = System.DateTime.Now.AddDays(30), IsClosed = false, ParentTask = null, ParentTaskId = null, Priority = 20, StartDate = System.DateTime.Now, TaskDescription = "Fake Taks", TaskId = 0 };
           // Arrange
           var result = Controller.AddTask(task);

           // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkNegotiatedContentResult<TaskModel>>(result);
        }

        [TestCase]
        public void When_Post_AnNewTaskAndDBError_Then_Fail()
        {
          //Arrange
           var task = new TaskModel { EndDate = System.DateTime.Now.AddDays(30), IsClosed = false, ParentTask = null, ParentTaskId = null, Priority = 20, StartDate = System.DateTime.Now, TaskDescription = "Fake Taks", TaskId = 0 };
            var service = Substitute.For<ITaskService>();
            service.AddTask(task).Returns(x => { return null; });
            Controller = new TaskManagerController(taskService);

          //Arrange
           var result = Controller.UpdateTaks(task);

           //ssert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<BadRequestResult>(result);
        }


        //[TestCase]
        //public void When_GetTaskModels_Then_MatchingTasksReceived()
        //{
        //    //Arrange & Act
        //    var result = Controller.AddTask(new TaskManagerQueryModel() { TaskName = "First Task" });
        //    //Assert
        //    Assert.IsNotNull(result);
        //    Assert.AreEqual(1, result.Count);
        //}


        [TestCase]
        public void When_ModelWithInvalidState_Then_ValidationShouldStopFurtherSteps()
        {
            //Arrange & Act
            var task = TaskServiceFakeData.TasksData.Task1.Clone();
            task.TaskDescription = null;
            task.Priority = 35;
            var context = new ValidationContext(task, null, null);
            var valResult = new List<ValidationResult>();
            var modelState = Validator.TryValidateObject(task, context, valResult, true); ;

            //Assert
            Assert.IsNotNull(valResult);
            Assert.True(!modelState);

        }

        [TestCase]
        public void When_CloseTaskWithInvalidModelState_Then_Should_Fail()
        {
            //Arrange
           var task = TaskServiceFakeData.TasksData.Task1.Clone();
            task.TaskDescription = null;
            task.Priority = 35;

            //Act
            Controller.ModelState.AddModelError("Priorty", "Priority is invalid");
            var result = Controller.CloseTask(task);

           // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<InvalidModelStateResult>(result);

        }

        [TestCase]
        public void When_CloseTaskWithIdMismatch_Then_Should_Fail()
        {
           // Arrange
           var task = TaskServiceFakeData.TasksData.Task1.Clone();
            task.TaskId = 100;
            //Act
           var result = Controller.CloseTask(task);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<BadRequestErrorMessageResult>(result);

        }

        [TestCase]
        public void When_CloseTaskWithValidModelState_Then_Should_Pass()
        {
            // Arrange
            var task = TaskServiceFakeData.TasksData.Task1;


           // Act
            Controller.ModelState.Clear();
            var result = Controller.CloseTask(task);

           // Assert           
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkNegotiatedContentResult<TaskModel>>(result);

        }
    }
}
