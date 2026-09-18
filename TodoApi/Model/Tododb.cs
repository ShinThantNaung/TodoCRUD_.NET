using Microsoft.EntityFrameworkCore;

namespace Todo.Model
{
    class TodoDb : DbContext
    {
        public TodoDb(DbContextOptions<TodoDb> options)
            : base(options) { }

        public DbSet<TodoItem> Todos => Set<TodoItem>();
    }
}