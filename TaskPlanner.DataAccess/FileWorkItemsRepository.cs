using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sereda.TaskPlanner.Domain.Models;
using Sereda.TaskPlanner.Domain.Models.Enums;
using Sereda.TaskPlanner.DataAccess.Abstractions;
using Newtonsoft.Json;


namespace Sereda.TaskPlanner.DataAccess
{
    public class FileWorkItemsRepository: IWorkItemsRepository 
    {

        private const string FilePath = "work-items.json"; 
        private readonly Dictionary<Guid, WorkItem> workItems; 

        public FileWorkItemsRepository()
        {
            workItems = new Dictionary<Guid, WorkItem>();
            if (File.Exists(FilePath))
            {
                var fileContent = File.ReadAllText(FilePath);
                if (!string.IsNullOrWhiteSpace(fileContent))
                {
                    var itemsArray = JsonConvert.DeserializeObject<WorkItem[]>(fileContent);
                    if (itemsArray != null)
                    {
                        foreach (var item in itemsArray)
                        {
                            workItems[item.id] = item; // workitem  Dictionary
                        }
                    }
                }
            }
        }


        public Guid Add(WorkItem workItem)
        {
            workItem.id = Guid.NewGuid(); // gen new ID
            workItems[workItem.id] = workItem; //add to Dictionary
            return workItem.id;
        }

        public WorkItem Get(Guid id)
        {
            workItems.TryGetValue(id, out var workItem);
            return workItem;
        }

        public WorkItem[] GetAll()
        {
            return new List<WorkItem>(workItems.Values).ToArray(); // return all WorkItems
        }

        public bool Update(WorkItem workItem)
        {
            if (workItems.ContainsKey(workItem.id))
            {
                workItems[workItem.id] = workItem; // update WorkItem
                return true;
            }
            return false;
        }

        public bool Remove(Guid id)
        {
            return workItems.Remove(id); // remove WorkItem from Dictionary
        }

        public void SaveChanges()
        {
            var itemsArray = workItems.Values.ToArray(); // converting 
            var json = JsonConvert.SerializeObject(itemsArray, Formatting.Indented);
            File.WriteAllText(FilePath, json); // writing JSON in file
        }
    }



}
