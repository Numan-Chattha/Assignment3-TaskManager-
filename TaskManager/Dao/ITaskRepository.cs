using System;
using System.Collections.Generic;
using System.Linq;
using TaskManager.Models;

namespace TaskManager.Dao
{
    public interface ITaskRepository
    {
        List<MyTask> GetAllTasks();
        List<MyTask> SearchForTasks(string searchText);
        void AddNewTask(MyTask myTask);
        MyTask getTaskByID(long ID);
        bool UpdateTask(MyTask myTask);
        bool RemoveTask(long ID);

    }
}
