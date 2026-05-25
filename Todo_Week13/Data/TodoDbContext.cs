using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Todo_Week13.Model;

namespace Todo_Week13.Data
{
    public class TodoDbContext : DbContext
    {

        public TodoDbContext(DbContextOptions<TodoDbContext> options) : base(options)
        {

        }
        public DbSet<TodoItem> TodoItems { get; set; }
    }
}
