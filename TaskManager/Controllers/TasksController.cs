using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TaskManager.Dao;
using TaskManager.Models;

namespace TaskManager.Controllers
{
    public class TasksController : Controller
    {
        private readonly ITaskRepository taskRepository;
        private readonly ILogger<TasksController> logger;

        public TasksController(ITaskRepository taskRepository, ILogger<TasksController> logger)
        {
            this.taskRepository = taskRepository;
            this.logger = logger;
        }
        // GET: Tasks
        public ActionResult Index()
        {
            ViewBag.Message = "Test Message";
            ViewBag.Tasks = this.taskRepository.GetAllTasks();
            return View("TasksList");
        }

        public ActionResult TasksList(string? search)
        {
            if (string.IsNullOrEmpty(search))
            {
                ViewBag.Tasks = this.taskRepository.GetAllTasks();
            }
            else 
            {
                ViewBag.SearchText = search;
                ViewBag.Tasks = this.taskRepository.SearchForTasks(search);
            }
            return View("TasksList");
        }

        // GET: Tasks/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Tasks/Create
        public ActionResult Upsert(long? id)
        {

            MyTask myTask;
            if (id == null || id < 1)
            {
                //create
                return View(new MyTask());
            }
            //update
            myTask = this.taskRepository.getTaskByID(id ?? default(int));
            if (myTask == null)
            {
                logger.LogError($"Unable to find Task with ID = {id}");
                throw new Exception($"Unable to find Task with ID = {id}");
            }
            return View("Upsert", myTask);
        }

        // POST: Tasks/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Upsert(MyTask myTask)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // TODO: Add insert logic here
                    if (myTask.ID == null || myTask.ID < 1)
                    {   
                        this.taskRepository.AddNewTask(myTask);
                    }
                    else
                    {
                       var success = this.taskRepository.UpdateTask(myTask);
                        if (!success) {
                            logger.LogError($"Unable to update Task with ID = {myTask.ID}");
                            throw new Exception($"Unable to update Task with ID = {myTask.ID}");
                        }
                    }

                    ViewBag.Tasks = this.taskRepository.GetAllTasks();
                    return View("TasksList");
                }
                else {
                    return View("Upsert", myTask);
                }
               
            }
            catch (Exception e)
            {
                logger.LogError("Unable to process Upsert request"+e.ToString());
                return View("Upsert",myTask);
            }
        }


        public ActionResult Delete(long id)
        {
            var myTask = this.taskRepository.getTaskByID(id);
            if (myTask == null)
            {
                logger.LogError($"Unable to find Task with ID = {id}");

                throw new Exception($"Unable to find Task with ID = {id}");
            }
            return View("Delete", myTask);
        }

        // POST: Default/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                if (id != 0)
                {
                    var success = this.taskRepository.RemoveTask(id);
                    if (!success)
                    {
                        logger.LogError($"Unable to find Task with ID = {id}");
                        throw new Exception($"Unable to find Task with ID = {id}");
                    }
                    ViewBag.Message = "Deleted Successfully";
                }
            }
            catch(Exception e)
            {
                logger.LogError("Unable to process Delete request" + e.ToString());
                throw new Exception("Unable to process Delete request"+e.ToString());
            }

            ViewBag.Tasks = this.taskRepository.GetAllTasks();
            return View("TasksList");
        }
        public IActionResult CustomErrorPage(int id)
        {
            if (id > 0)
            {
                ViewBag.msg = "Invalid URL!  Page not found!";
            }
            else
            {
                ViewBag.msg = "An Error Occurred!";
            }
            return View("Error", ViewBag);
        }
        public IActionResult Privacy()
        {
            return View();
        }
    }
}