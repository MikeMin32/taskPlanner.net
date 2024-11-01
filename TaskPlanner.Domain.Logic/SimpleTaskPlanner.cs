using Sereda.TaskPlanner.Domain.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sereda.TaskPlanner.DataAccess.Abstractions; 

namespace Sereda.TaskPlanner.Domain.Logic
{
    public class SimpleTaskPlanner
    {

        private readonly IWorkItemsRepository _repository;

        public SimpleTaskPlanner(IWorkItemsRepository repository)
        { 
            _repository = repository;

        }
        // Метод для створення плану, витягуючи дані з репозиторію
        public WorkItem[] CreatePlan()
        {
            // Отримуємо всі задачі з репозиторія
            var items = _repository.GetAll();

            if (items == null || items.Length == 0)
            {
                Console.WriteLine("No tasks available to create a plan.");
                Console.WriteLine();
                return Array.Empty<WorkItem>();
            }

            // Фільтруємо виконані задачі
            var incompleteItems = items.Where(item => !item.IsCompleted).ToList();

            if (incompleteItems.Count == 0)
            {
                Console.WriteLine("All tasks are completed. No plan to create.");
                return Array.Empty<WorkItem>();
            }

            // Сортуємо задачі
            incompleteItems.Sort(CompareWorkItems);

            return incompleteItems.ToArray();
        }

        private static int CompareWorkItems(WorkItem firstItem, WorkItem secondItem)
        {
            int priorityComparison = secondItem.Priority.CompareTo(firstItem.Priority);
            if (priorityComparison != 0)
                return priorityComparison;

            int dueDateComparison = firstItem.DueDate.CompareTo(secondItem.DueDate);
            if (dueDateComparison != 0)
                return dueDateComparison;

            return string.Compare(firstItem.Title, secondItem.Title, StringComparison.OrdinalIgnoreCase);
        }
    }
}
