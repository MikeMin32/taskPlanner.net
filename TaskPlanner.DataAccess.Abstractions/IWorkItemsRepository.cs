using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sereda.TaskPlanner.Domain.Models.Enums;

namespace Sereda.TaskPlanner.DataAccess.Abstractions
{
    public interface IWorkItemsRepository
    {
        Guid Add(WorkItem workItem);
        WorkItem Get(Guid id);
        WorkItem[] GetAll();

        bool Update(WorkItem workItem);
        bool Remove (Guid id);

        void SaveChanges();
    }
}
