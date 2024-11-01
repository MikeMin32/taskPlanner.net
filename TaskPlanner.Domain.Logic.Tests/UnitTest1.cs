using Moq;
using Sereda.TaskPlanner.DataAccess;
using Sereda.TaskPlanner.DataAccess.Abstractions;
using Sereda.TaskPlanner.Domain.Models.Enums;

namespace Sereda.TaskPlanner.Domain.Logic.Tests
{
    public class SimpleTaskPlannerTests
    {
        [Fact]
        public void CreatePlan_ShouldSortTasksCorrectly()
        {
            // Arrange
            var mockRepository = new Mock<IWorkItemsRepository>();

            var workItems = new WorkItem[]
            {
                new WorkItem { Title = "Task 1", DueDate = DateTime.Now.AddDays(3), Priority = Priority.Medium, IsCompleted = false },
                new WorkItem { Title = "Task 2", DueDate = DateTime.Now.AddDays(1), Priority = Priority.High, IsCompleted = false },
                new WorkItem { Title = "Task 3", DueDate = DateTime.Now.AddDays(2), Priority = Priority.Low, IsCompleted = false }
            };

            mockRepository.Setup(repo => repo.GetAll()).Returns(workItems);

            var taskPlanner = new SimpleTaskPlanner(mockRepository.Object);

            // Act
            var plan = taskPlanner.CreatePlan();

            // Assert
            Assert.Equal("Task 2", plan[0].Title); // Highest priority and soonest due date
            Assert.Equal("Task 1", plan[1].Title); // Medium priority, later due date than Task 2
            Assert.Equal("Task 3", plan[2].Title); // Lowest priority
        }

        [Fact]
        public void CreatePlan_ShouldIncludeOnlyIncompleteTasks()
        {
            // Arrange
            var mockRepository = new Mock<IWorkItemsRepository>();

            var workItems = new WorkItem[]
            {
                new WorkItem { Title = "Task 1", DueDate = DateTime.Now.AddDays(1), Priority = Priority.High, IsCompleted = false },
                new WorkItem { Title = "Task 2", DueDate = DateTime.Now.AddDays(2), Priority = Priority.Medium, IsCompleted = true }, // Completed task
                new WorkItem { Title = "Task 3", DueDate = DateTime.Now.AddDays(3), Priority = Priority.Low, IsCompleted = false }
            };

            mockRepository.Setup(repo => repo.GetAll()).Returns(workItems);

            var taskPlanner = new SimpleTaskPlanner(mockRepository.Object);

            // Act
            var plan = taskPlanner.CreatePlan();

            // Assert
            Assert.Equal(2, plan.Length); // Only 2 tasks should be included in the plan
            Assert.DoesNotContain(plan, item => item.Title == "Task 2"); // Completed task should be excluded
        }

        [Fact]
        public void CreatePlan_ShouldReturnEmptyIfAllTasksAreCompleted()
        {
            // Arrange
            var mockRepository = new Mock<IWorkItemsRepository>();

            var workItems = new WorkItem[]
            {
                new WorkItem { Title = "Task 1", DueDate = DateTime.Now.AddDays(1), Priority = Priority.High, IsCompleted = true },
                new WorkItem { Title = "Task 2", DueDate = DateTime.Now.AddDays(2), Priority = Priority.Medium, IsCompleted = true }
            };

            mockRepository.Setup(repo => repo.GetAll()).Returns(workItems);

            var taskPlanner = new SimpleTaskPlanner(mockRepository.Object);

            // Act
            var plan = taskPlanner.CreatePlan();

            // Assert
            Assert.Empty(plan); // Since all tasks are completed, plan should be empty
        }
        [Fact]
        public void CreatePlan_ShouldIgnoreCompletedTasks()
        {
            // Arrange
            var mockRepository = new Mock<IWorkItemsRepository>();

            var workItems = new WorkItem[]
            {
        new WorkItem { Title = "Task 1", DueDate = DateTime.Now.AddDays(1), Priority = Priority.High, IsCompleted = true },  // Completed task
        new WorkItem { Title = "Task 2", DueDate = DateTime.Now.AddDays(2), Priority = Priority.Medium, IsCompleted = false }, // Incomplete task
        new WorkItem { Title = "Task 3", DueDate = DateTime.Now.AddDays(3), Priority = Priority.Low, IsCompleted = false }    // Incomplete task
            };

            mockRepository.Setup(repo => repo.GetAll()).Returns(workItems);

            var taskPlanner = new SimpleTaskPlanner(mockRepository.Object);

            // Act
            var plan = taskPlanner.CreatePlan();

            // Assert
            Assert.Equal(2, plan.Length); // Only 2 tasks should be included in the plan (since one is completed)
            Assert.DoesNotContain(plan, item => item.IsCompleted); // No completed tasks in the plan
        }

        [Fact]
        public void CreatePlan_ShouldSortTasksByDueDateIfPrioritiesAreEqual()
        {
            // Arrange
            var mockRepository = new Mock<IWorkItemsRepository>();

            var workItems = new WorkItem[]
            {
        new WorkItem { Title = "Task 1", DueDate = DateTime.Now.AddDays(3), Priority = Priority.Medium, IsCompleted = false },
        new WorkItem { Title = "Task 2", DueDate = DateTime.Now.AddDays(1), Priority = Priority.Medium, IsCompleted = false },
        new WorkItem { Title = "Task 3", DueDate = DateTime.Now.AddDays(2), Priority = Priority.Medium, IsCompleted = false }
            };

            mockRepository.Setup(repo => repo.GetAll()).Returns(workItems);

            var taskPlanner = new SimpleTaskPlanner(mockRepository.Object);

            // Act
            var plan = taskPlanner.CreatePlan();

            // Assert
            Assert.Equal("Task 2", plan[0].Title); // Soonest due date should come first
            Assert.Equal("Task 3", plan[1].Title); // Next due date
            Assert.Equal("Task 1", plan[2].Title); // Latest due date
        }

        [Fact]
        public void CreatePlan_ShouldReturnEmptyPlanIfAllTasksAreCompleted()
        {
            // Arrange
            var mockRepository = new Mock<IWorkItemsRepository>();

            var workItems = new WorkItem[]
            {
        new WorkItem { Title = "Task 1", DueDate = DateTime.Now.AddDays(1), Priority = Priority.High, IsCompleted = true },
        new WorkItem { Title = "Task 2", DueDate = DateTime.Now.AddDays(2), Priority = Priority.Medium, IsCompleted = true },
        new WorkItem { Title = "Task 3", DueDate = DateTime.Now.AddDays(3), Priority = Priority.Low, IsCompleted = true }
            };

            mockRepository.Setup(repo => repo.GetAll()).Returns(workItems);

            var taskPlanner = new SimpleTaskPlanner(mockRepository.Object);

            // Act
            var plan = taskPlanner.CreatePlan();

            // Assert
            Assert.Empty(plan); // Since all tasks are completed, plan should be empty
        }
        


    }
}