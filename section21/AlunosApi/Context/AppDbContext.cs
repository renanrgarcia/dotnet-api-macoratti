using AlunosApi.Models;
using Microsoft.EntityFrameworkCore;

namespace AlunosApi.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Aluno> Alunos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Aluno>().HasData(
                new Aluno()
                {
                    Id = 1,
                    Nome = "João",
                    Email = "joao@ig.com.br",
                    Idade = 20
                },
                new Aluno()
                {
                    Id = 2,
                    Nome = "Maria",
                    Email = "maria@ig.com.br",
                    Idade = 25
                }
            );
        }
    }
}