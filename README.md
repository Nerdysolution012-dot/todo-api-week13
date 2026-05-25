# todo-api-week13

Advanced TODO API built with ASP.NET Core Web API and Entity Framework Core.

## Features

- CRUD operations for TODO items
- Database seeding with 10 sample TODO items
- Filtering by status
- Filtering by priority
- Combined filtering
- Search by title and description
- Sorting by title, due date, priority, and created date
- Pagination with metadata
- Statistics endpoint

## Tech Stack

- ASP.NET Core Web API
- Entity Framework Core
- SQLite

## Setup Instructions

### 1. Clone the repository
```bash
git clone https://github.com/your-username/todo-api-week13.git
cd todo-api-week13
```

### 2. Restore packages
```bash
dotnet restore
```

### 3. Run the project
```bash
dotnet run
```

### 4. Open Swagger
Use the local Swagger URL shown in the terminal.

## Database Setup

The application uses SQLite.

On startup:
- the database is created automatically if it does not exist
- the app seeds 10 sample TODO items if the table is empty

## Project Structure

```text
todo-api-week13/
├── Controllers/
│   └── TodosController.cs
├── Data/
│   ├── TodoDbContext.cs
│   └── DatabaseSeeder.cs
├── Models/
│   └── TodoItem.cs
├── DTOs/
│   ├── CreateTodoDto.cs
│   ├── UpdateTodoDto.cs
│   └── TodoResponseDto.cs
├── Responses/
│   └── ErrorResponse.cs
├── Program.cs
├── appsettings.json
└── README.md
```

## API Endpoints

### Get all TODOs
`GET /api/todos`

Optional query parameters:
- `status=completed|pending`
- `priority=High|Medium|Low`
- `sortBy=title|duedate|priority|createdat`
- `sortOrder=asc|desc`
- `page=1`
- `pageSize=10`

### Get TODO by ID
`GET /api/todos/{id}`

### Create TODO
`POST /api/todos`

Example request body:
```json
{
  "title": "Finish homework",
  "description": "Complete LINQ exercises",
  "priority": "High",
  "isCompleted": false,
  "dueDate": "2026-05-30T00:00:00Z"
}
```

### Update TODO
`PUT /api/todos/{id}`

### Delete TODO
`DELETE /api/todos/{id}`

### Search TODOs
`GET /api/todos/search?q=meeting`

### Statistics
`GET /api/todos/stats`

## Query Examples

### Filtering Examples
- Get completed TODOs: `GET /api/todos?status=completed`
- Get high priority TODOs: `GET /api/todos?priority=High`
- Get pending high priority: `GET /api/todos?status=pending&priority=High`

### Search Examples
- Search for "meeting": `GET /api/todos/search?q=meeting`
- Search for "project": `GET /api/todos/search?q=project`
- Search for "bug": `GET /api/todos/search?q=bug`

### Sorting Examples
- Sort by title: `GET /api/todos?sortBy=title`
- Sort by due date descending: `GET /api/todos?sortBy=duedate&sortOrder=desc`
- Sort by priority ascending: `GET /api/todos?sortBy=priority&sortOrder=asc`
- Sort by created date descending: `GET /api/todos?sortBy=createdat&sortOrder=desc`

### Pagination Examples
- Page 1, 10 items: `GET /api/todos?page=1&pageSize=10`
- Page 2, 5 items: `GET /api/todos?page=2&pageSize=5`
- Pending todos with pagination: `GET /api/todos?status=pending&page=1&pageSize=20`

### Combined Examples
- High priority, sorted by due date, page 1:
  `GET /api/todos?priority=High&sortBy=duedate&sortOrder=asc&page=1&pageSize=10`

## Database Seeding

The project includes a `DatabaseSeeder` class in `Data/DatabaseSeeder.cs`.

Seeded data includes:
- 10 sample TODO items
- completed and pending items
- High, Medium, and Low priorities
- past, current, and future due dates
- varied creation dates

The seeder only runs when the database has no existing TODO records.

## Statistics Response Example

```json
{
  "total": 10,
  "completed": 3,
  "pending": 7,
  "completionRate": 30.00,
  "byStatus": {
    "completed": 3,
    "pending": 7
  },
  "byPriority": {
    "high": 4,
    "medium": 4,
    "low": 2
  },
  "dueToday": 1,
  "overdue": 1
}
```

## Testing Checklist

- [x] Database seeds on startup
- [x] Status filter works
- [x] Priority filter works
- [x] Combined filters work
- [x] Search works in title and description
- [x] Search is case-insensitive
- [x] Sorting works
- [x] Pagination works
- [x] Statistics endpoint works

## Suggested Postman / Thunder Client Requests

1. `GET /api/todos`
2. `GET /api/todos?status=completed`
3. `GET /api/todos?status=pending`
4. `GET /api/todos?priority=High`
5. `GET /api/todos?priority=Medium`
6. `GET /api/todos?priority=Low`
7. `GET /api/todos?status=pending&priority=High`
8. `GET /api/todos/search?q=meeting`
9. `GET /api/todos/search?q=project`
10. `GET /api/todos/search?q=bug`
11. `GET /api/todos?sortBy=title`
12. `GET /api/todos?sortBy=duedate&sortOrder=desc`
13. `GET /api/todos?sortBy=priority&sortOrder=asc`
14. `GET /api/todos?sortBy=createdat&sortOrder=desc`
15. `GET /api/todos?page=1&pageSize=5`
16. `GET /api/todos?page=2&pageSize=5`
17. `GET /api/todos?status=pending&page=1&pageSize=20`
18. `GET /api/todos?priority=High&sortBy=duedate&sortOrder=asc&page=1&pageSize=10`
19. `GET /api/todos/stats`
20. `POST /api/todos`

## Submission Notes

Make sure you also include:
- screenshots from Postman or Thunder Client
- at least 12 meaningful commits
- the GitHub repository link in Google Classroom
