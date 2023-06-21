using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class MealDbContext : DbContext
    {

        public MealDbContext(DbContextOptions<MealDbContext> options) : base(options)
        {

        }

        public DbSet<Package> Packages { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Student> Students { get; set; }

        public DbSet<Canteen> Canteens { get; set; }

        public DbSet<Employee> Employees { get; set; }

    }
}
