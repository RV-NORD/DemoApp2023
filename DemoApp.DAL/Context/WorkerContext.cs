using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DemoApp.DAL.Entityes;
using Microsoft.EntityFrameworkCore;

namespace DemoApp.DAL.Context
{
    public class WorkerContext : DbContext
    {
        public DbSet<Worker> Worker { get; set; }

        public DbSet<Child> Childs { get; set; }
        public DbSet<WorkerChildCountStatistic> Stat001 { get; set; }

        
        
        
        public WorkerContext(DbContextOptions<WorkerContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            

            modelBuilder.Entity<Worker>().HasData(
                    new Worker { Id = 1, SurName = "Иванов", FirstName = "Иван", LastName = "Иванович", BirthDay = new DateOnly(2000,10,10), Pol=true },
                    new Worker { Id = 2, SurName = "Петров", FirstName = "Пётр", LastName = "Петрович", BirthDay = new DateOnly(1950, 10, 10), Pol = true },
                    new Worker { Id = 3, SurName = "Фёдоров", FirstName = "Фёдор", LastName = "Фёдорович", BirthDay = new DateOnly(1980, 10, 10), Pol = true }
            );

            modelBuilder.Entity<Child>().HasData(
                new Child { Id = 1, FullName = "Ребёнок 1", BirthDay = new DateOnly(2000, 1, 10), WorkerId = 1 },
                new Child { Id = 2, FullName = "Ребёнок 2", BirthDay = new DateOnly(2000, 2, 10), WorkerId = 1 },
                new Child { Id = 3, FullName = "Ребёнок 3", BirthDay = new DateOnly(2000, 3, 10), WorkerId = 2 },
                new Child { Id = 4, FullName = "Ребёнок 4", BirthDay = new DateOnly(2000, 4, 10), WorkerId = 2 },
                new Child { Id = 5, FullName = "Ребёнок 5", BirthDay = new DateOnly(2000, 5, 10), WorkerId = 3 },
                new Child { Id = 6, FullName = "Ребёнок 6", BirthDay = new DateOnly(2000, 6, 10), WorkerId = 3 },
                new Child { Id = 7, FullName = "Ребёнок 7", BirthDay = new DateOnly(2000, 7, 10), WorkerId = 3 },
                new Child { Id = 8, FullName = "Ребёнок 8", BirthDay = new DateOnly(2000, 8, 10), WorkerId = 3 }
            );
        }
    }
}
