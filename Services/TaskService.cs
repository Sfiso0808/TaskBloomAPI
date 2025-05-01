using TaskBloomAPI.Models;
using MySql.Data.MySqlClient;
using TaskModel = TaskBloomAPI.Models.Task;
using System.Collections.Generic;

namespace TaskBloomAPI.Services
{
    public class TaskService
    {
        private readonly string _connectionString = null!;

        public TaskService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        public List<TaskBloomAPI.Models.Task> GetAllTasks()
        {
            List<TaskBloomAPI.Models.Task> tasks = new();

            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM tasks";
                using (var cmd = new MySqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        TaskBloomAPI.Models.Task task = new()
                        {
                            Id = reader.GetInt32("id"),
                            Title = reader.GetString("title"),
                            Description = reader.GetString("description"),
                            DueDate = reader.GetDateTime("due_date"),
                            Priority = reader.GetInt32("priority"),
                            IsCompleted = reader.GetBoolean("is_completed")
                        };
                        tasks.Add(task);
                    }
                }
            }

            return tasks;
        }

        public void AddTask(TaskBloomAPI.Models.Task task)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                string query = @"INSERT INTO tasks (title, description, due_date, priority, is_completed) 
                                 VALUES (@Title, @Description, @DueDate, @Priority, @IsCompleted)";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Title", task.Title);
                    cmd.Parameters.AddWithValue("@Description", task.Description);
                    cmd.Parameters.AddWithValue("@DueDate", task.DueDate);
                    cmd.Parameters.AddWithValue("@Priority", task.Priority);
                    cmd.Parameters.AddWithValue("@IsCompleted", task.IsCompleted);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void UpdateTask(TaskBloomAPI.Models.Task task)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                string query = @"UPDATE tasks 
                                 SET title = @Title, description = @Description, due_date = @DueDate, 
                                     priority = @Priority, is_completed = @IsCompleted 
                                 WHERE id = @Id";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Title", task.Title);
                    cmd.Parameters.AddWithValue("@Description", task.Description);
                    cmd.Parameters.AddWithValue("@DueDate", task.DueDate);
                    cmd.Parameters.AddWithValue("@Priority", task.Priority);
                    cmd.Parameters.AddWithValue("@IsCompleted", task.IsCompleted);
                    cmd.Parameters.AddWithValue("@Id", task.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteTask(int id)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                string query = "DELETE FROM tasks WHERE id = @Id";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void MarkComplete(int id)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                string query = "UPDATE tasks SET is_completed = 1 WHERE id = @Id";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
