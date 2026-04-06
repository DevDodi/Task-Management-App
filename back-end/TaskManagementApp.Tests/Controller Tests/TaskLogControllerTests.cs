using Xunit;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TaskManagementApp.Controllers;
using TaskManagementApp.Services.Interfaces;
using TaskManagementApp.Services.Responses;
using TaskManagementApp.Models;

namespace TaskManagementApp.Tests
{
    public class TaskLogControllerTests
    {
        private readonly Mock<ITaskLogService> taskLogServiceMock = new();

        private TaskLogController CreateController() =>
            new TaskLogController(taskLogServiceMock.Object);

        [Fact]
        public async System.Threading.Tasks.Task GetTaskLogs_ReturnsBadRequest_WhenTaskIdEmpty()
        {
            var controller = CreateController();
            var result = await controller.GetTaskLogs(Guid.Empty, null, null);
            Assert.IsType<BadRequestResult>(result.Result);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetTaskLogs_ReturnsBadRequest_WhenServiceFails()
        {
            var taskId = Guid.NewGuid();
            taskLogServiceMock
                .Setup(s => s.GetTaskLogsAsync(taskId, null, null))
                .ReturnsAsync(new TaskLogServiceResponseList(false, null, "fail"));

            var controller = CreateController();
            var result = await controller.GetTaskLogs(taskId, null, null);
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetTaskLogs_ReturnsOk_WhenServiceSucceeds()
        {
            var taskId = Guid.NewGuid();
            var logs = new List<TaskLog> { new TaskLog { Id = Guid.NewGuid(), TaskId = taskId } };
            taskLogServiceMock
                .Setup(s => s.GetTaskLogsAsync(taskId, null, null))
                .ReturnsAsync(new TaskLogServiceResponseList(true, logs));

            var controller = CreateController();
            var result = await controller.GetTaskLogs(taskId, null, null);
            Assert.IsType<OkObjectResult>(result.Result);
        }
    }
}
