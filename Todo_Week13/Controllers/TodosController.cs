using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Todo_Week13.Data;
using Todo_Week13.DTOs;
using Todo_Week13.Model;
using Todo_Week13.Response;

namespace Todo_Week13.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodosController : ControllerBase
    {
        private readonly TodoDbContext _context;

        public TodosController(TodoDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<object>> GetTodos(
            [FromQuery] string? status,
            [FromQuery] string? priority,
            [FromQuery] string? sortBy,
            [FromQuery] string? sortOrder,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;
            if (pageSize > 100) pageSize = 100;

            var query = _context.TodoItems.AsQueryable();

            if (!string.IsNullOrWhiteSpace(status))
            {
                if (status.Equals("completed", StringComparison.OrdinalIgnoreCase))
                {
                    query = query.Where(t => t.IsCompleted);
                }
                else if (status.Equals("pending", StringComparison.OrdinalIgnoreCase))
                {
                    query = query.Where(t => !t.IsCompleted);
                }
            }

            if (!string.IsNullOrWhiteSpace(priority))
            {
                query = query.Where(t => t.Priority.ToLower() == priority.ToLower());
            }

            var totalCount = await query.CountAsync();

            bool descending = sortOrder?.Equals("desc", StringComparison.OrdinalIgnoreCase) == true;

            query = sortBy?.ToLower() switch
            {
                "title" => descending
                    ? query.OrderByDescending(t => t.Title)
                    : query.OrderBy(t => t.Title),

                "duedate" => descending
                    ? query.OrderByDescending(t => t.DueDate)
                    : query.OrderBy(t => t.DueDate),

                "priority" => descending
                    ? query.OrderByDescending(t => t.Priority)
                    : query.OrderBy(t => t.Priority),

                "createdat" => descending
                    ? query.OrderByDescending(t => t.CreatedAt)
                    : query.OrderBy(t => t.CreatedAt),

                _ => query.OrderBy(t => t.Id)
            };

            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            var skip = (page - 1) * pageSize;

            var todos = await query
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();

            var response = new
            {
                Data = todos.Select(MapToDto),
                Pagination = new
                {
                    CurrentPage = page,
                    PageSize = pageSize,
                    TotalPages = totalPages,
                    TotalCount = totalCount,
                    HasPrevious = page > 1,
                    HasNext = page < totalPages
                }
            };

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TodoResponseDto>> GetTodo(int id)
        {
            var todo = await _context.TodoItems.FindAsync(id);

            if (todo == null)
            {
                return NotFound(new ErrorResponse
                {
                    StatusCode = 404,
                    Message = $"Todo with ID {id} not found",
                    Timestamp = DateTime.UtcNow
                });
            }

            return Ok(MapToDto(todo));
        }

        [HttpGet("search")]
        public async Task<ActionResult<object>> SearchTodos([FromQuery] string q)
        {
            if (string.IsNullOrWhiteSpace(q))
            {
                return BadRequest(new ErrorResponse
                {
                    StatusCode = 400,
                    Message = "Search query cannot be empty",
                    Timestamp = DateTime.UtcNow
                });
            }

            q = q.Trim().ToLower();

            var todos = await _context.TodoItems
                .Where(t =>
                    t.Title.ToLower().Contains(q) ||
                    (t.Description != null && t.Description.ToLower().Contains(q)))
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();

            var results = todos.Select(MapToDto).ToList();

            return Ok(new
            {
                Query = q,
                Count = results.Count,
                Results = results
            });
        }

        [HttpGet("stats")]
        public async Task<ActionResult<object>> GetStatistics()
        {
            var totalCount = await _context.TodoItems.CountAsync();
            var completedCount = await _context.TodoItems.CountAsync(t => t.IsCompleted);
            var pendingCount = totalCount - completedCount;

            var dueTodayCount = await _context.TodoItems
                .CountAsync(t => t.DueDate.HasValue &&
                                 t.DueDate.Value.Date == DateTime.Today);

            var overdueCount = await _context.TodoItems
                .CountAsync(t => t.DueDate.HasValue &&
                                 t.DueDate.Value.Date < DateTime.Today &&
                                 !t.IsCompleted);

            var highPriorityCount = await _context.TodoItems.CountAsync(t => t.Priority == "High");
            var mediumPriorityCount = await _context.TodoItems.CountAsync(t => t.Priority == "Medium");
            var lowPriorityCount = await _context.TodoItems.CountAsync(t => t.Priority == "Low");

            var stats = new
            {
                Total = totalCount,
                Completed = completedCount,
                Pending = pendingCount,
                CompletionRate = totalCount > 0
                    ? Math.Round((completedCount / (double)totalCount) * 100, 2)
                    : 0,
                ByStatus = new
                {
                    Completed = completedCount,
                    Pending = pendingCount
                },
                ByPriority = new
                {
                    High = highPriorityCount,
                    Medium = mediumPriorityCount,
                    Low = lowPriorityCount
                },
                DueToday = dueTodayCount,
                Overdue = overdueCount
            };

            return Ok(stats);
        }

        [HttpPost]
        public async Task<ActionResult<TodoResponseDto>> CreateTodo(CreateTodoDto dto)
        {
            var todo = new TodoItem
            {
                Title = dto.Title,
                Description = dto.Description,
                Priority = dto.Priority,
                IsCompleted = dto.IsCompleted,
                CreatedAt = DateTime.UtcNow,
                DueDate = dto.DueDate
            };

            _context.TodoItems.Add(todo);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTodo), new { id = todo.Id }, MapToDto(todo));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<TodoResponseDto>> UpdateTodo(int id, UpdateTodoDto dto)
        {
            var todo = await _context.TodoItems.FindAsync(id);

            if (todo == null)
            {
                return NotFound(new ErrorResponse
                {
                    StatusCode = 404,
                    Message = $"Todo with ID {id} not found",
                    Timestamp = DateTime.UtcNow
                });
            }

            todo.Title = dto.Title;
            todo.Description = dto.Description;
            todo.Priority = dto.Priority;
            todo.IsCompleted = dto.IsCompleted;
            todo.DueDate = dto.DueDate;

            await _context.SaveChangesAsync();

            return Ok(MapToDto(todo));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTodo(int id)
        {
            var todo = await _context.TodoItems.FindAsync(id);

            if (todo == null)
            {
                return NotFound(new ErrorResponse
                {
                    StatusCode = 404,
                    Message = $"Todo with ID {id} not found",
                    Timestamp = DateTime.UtcNow
                });
            }

            _context.TodoItems.Remove(todo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private static TodoResponseDto MapToDto(TodoItem todo)
        {
            return new TodoResponseDto
            {
                Id = todo.Id,
                Title = todo.Title,
                Description = todo.Description,
                Priority = todo.Priority,
                IsCompleted = todo.IsCompleted,
                CreatedAt = todo.CreatedAt,
                DueDate = todo.DueDate
            };
        }
    }
}
