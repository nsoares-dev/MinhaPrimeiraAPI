using Microsoft.EntityFrameworkCore;

namespace MinimalAPI
{
    public class TodoDB : DbContext
    {
        public TodoDB(DbContextOptions<TodoDB> options) 
            : base(options) { }

        public DbSet<Todo> todos => Set<Todo>();
        public DbSet<Aluno> alunos => Set<Aluno>();
    }
}
