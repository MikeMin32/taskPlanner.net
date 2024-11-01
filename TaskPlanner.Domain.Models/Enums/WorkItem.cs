using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sereda.TaskPlanner.Domain.Models.Enums
{
    public class WorkItem
    {
        public DateTime CreationDate { get; set; }
        public DateTime DueDate { get; set; }
        public Priority Priority { get; set; }
        public Complexity Complexity { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }

        public Guid id { get; set; } //id

        public WorkItem()
        {
            id = Guid.NewGuid(); //unic. id
        }

        public WorkItem Clone()
        {
            return new WorkItem
            {
                id = this.id,
                CreationDate = this.CreationDate,
                DueDate = this.DueDate,
                Priority = this.Priority,
                Complexity = this.Complexity,
                Title = this.Title,
                Description = this.Description,
                IsCompleted = this.IsCompleted,

            };

        }

        public override string ToString()
        {
            return $"ID: {id}, {Title}: due {DueDate:dd.MM.yyyy}, {Priority.ToString().ToLower()} priority";
        }
    }
}
