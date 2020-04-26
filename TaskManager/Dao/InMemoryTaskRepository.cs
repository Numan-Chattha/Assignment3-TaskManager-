using System;
using System.Collections.Generic;
using System.Linq;
using TaskManager.Models;

namespace TaskManager.Dao
{
    public class InMemoryTaskRepository : ITaskRepository
    {
        List<MyTask> MyTasksList = new List<MyTask>();
        public List<MyTask> GetAllTasks()
        {
            return MyTasksList.OrderByDescending(task => task.ID).ToList();
        }

        public void AddNewTask(MyTask myTask)
        {
            myTask.ID = GetAutoGenID();
             MyTasksList.Add(myTask);
        }

        public MyTask getTaskByID(long ID)
        {
            return MyTasksList.SingleOrDefault(task => task.ID == ID);
        }

        private long GetAutoGenID()
        {
            var latestTask = MyTasksList.OrderByDescending(item => item.ID).FirstOrDefault();
            var ID = latestTask == null ? 1 : latestTask.ID + 1;
            return ID;
        }

        public bool UpdateTask(MyTask myTask)
        {
            var index = MyTasksList.FindIndex(task => task.ID == myTask.ID);
            if (index > -1) {
                MyTasksList[index] = myTask;
                return true;
            }
            else {
                return false;
            }
        }

        public bool RemoveTask(long ID)
        {
            var removed = MyTasksList.RemoveAll(task => task.ID == ID);
            if(removed ==1)
                return true;
            return false;

        }

        public List<MyTask> SearchForTasks(string searchText)
        {   

            var result = from task in MyTasksList
                         where task.Title.ToLower().Contains(searchText.ToLower())
                         select task;
            return result.ToList();
        }
    }
}
