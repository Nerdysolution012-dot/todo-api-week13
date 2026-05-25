using Todo_Week13.Model;

namespace Todo_Week13.Data
{
    public static class DataBaseSeeder
    {
        public static void SeedData(TodoDbContext context)
        {
            if (context.TodoItems.Any())
            {
                return;
            }

            var todos = new List<TodoItem>
            {
                new TodoItem
                {
                    Title = "Complete project proposal",
                    Description = "Finish writing the Q4 project proposal document",
                    Priority = "High",
                    IsCompleted = false,
                    CreatedAt = DateTime.UtcNow.AddDays(-10),
                    DueDate = DateTime.UtcNow.AddDays(2)
                },
                new TodoItem
                {
                    Title = "Team meeting",
                    Description = "Weekly sync with development team",
                    Priority = "Medium",
                    IsCompleted = true,
                    CreatedAt = DateTime.UtcNow.AddDays(-5),
                    DueDate = DateTime.UtcNow.AddDays(-3)
                },
                new TodoItem
                {
                    Title = "Code review",
                    Description = "Review pull requests from team members",
                    Priority = "High",
                    IsCompleted = false,
                    CreatedAt = DateTime.UtcNow.AddDays(-7),
                    DueDate = DateTime.UtcNow.AddDays(1)
                },
                new TodoItem
                {
                    Title = "Update documentation",
                    Description = "Update API documentation with new endpoints",
                    Priority = "Low",
                    IsCompleted = false,
                    CreatedAt = DateTime.UtcNow.AddDays(-15),
                    DueDate = DateTime.UtcNow.AddDays(7)
                },
                new TodoItem
                {
                    Title = "Fix bug #234",
                    Description = "Resolve the authentication timeout issue",
                    Priority = "High",
                    IsCompleted = true,
                    CreatedAt = DateTime.UtcNow.AddDays(-20),
                    DueDate = DateTime.UtcNow.AddDays(-18)
                },
                new TodoItem
                {
                    Title = "Database backup",
                    Description = "Perform monthly database backup",
                    Priority = "Medium",
                    IsCompleted = false,
                    CreatedAt = DateTime.UtcNow.AddDays(-2),
                    DueDate = DateTime.UtcNow.AddDays(5)
                },
                new TodoItem
                {
                    Title = "Client presentation",
                    Description = "Present demo to stakeholders",
                    Priority = "High",
                    IsCompleted = false,
                    CreatedAt = DateTime.UtcNow.AddDays(-8),
                    DueDate = DateTime.UtcNow
                },
                new TodoItem
                {
                    Title = "Refactor authentication module",
                    Description = "Improve code quality in auth module",
                    Priority = "Low",
                    IsCompleted = false,
                    CreatedAt = DateTime.UtcNow.AddDays(-12),
                    DueDate = DateTime.UtcNow.AddDays(14)
                },
                new TodoItem
                {
                    Title = "Write unit tests",
                    Description = "Add unit tests for new features",
                    Priority = "Medium",
                    IsCompleted = true,
                    CreatedAt = DateTime.UtcNow.AddDays(-6),
                    DueDate = DateTime.UtcNow.AddDays(-4)
                },
                new TodoItem
                {
                    Title = "Performance optimization",
                    Description = "Optimize database queries for better performance",
                    Priority = "Medium",
                    IsCompleted = false,
                    CreatedAt = DateTime.UtcNow.AddDays(-3),
                    DueDate = DateTime.UtcNow.AddDays(10)
                }
            };

            context.TodoItems.AddRange(todos);
            context.SaveChanges();
        }
    }

}
