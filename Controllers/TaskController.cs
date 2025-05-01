using Microsoft.AspNetCore.Mvc;
using TaskBloomAPI.Services;
using TaskModel = TaskBloomAPI.Models.Task;

namespace TaskBloomAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly TaskService _taskService;

        public TaskController(TaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet]
        public ActionResult<List<TaskModel>> GetAllTasks()
        {
            try
            {
                return _taskService.GetAllTasks();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public ActionResult<TaskModel> GetTaskById(int id)
        {
            try
            {
                var task = _taskService.GetAllTasks().FirstOrDefault(t => t.Id == id);

                if (task == null)
                    return NotFound();

                return task;
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server error: {ex.Message}");
            }
        }

        [HttpPost]
        public IActionResult AddTask(TaskModel task)
        {
            try
            {
                _taskService.AddTask(task);
                return CreatedAtAction(nameof(GetTaskById), new { id = task.Id }, task);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateTask(int id, TaskModel updatedTask)
        {
            try
            {
                updatedTask.Id = id;
                _taskService.UpdateTask(updatedTask);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTask(int id)
        {
            try
            {
                _taskService.DeleteTask(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server error: {ex.Message}");
            }
        }
    }
}
