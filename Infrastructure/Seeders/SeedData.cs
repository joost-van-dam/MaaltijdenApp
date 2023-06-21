using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace Infrastructure.Seeders
{
    public class SeedData : ISeedData
    {
        private MealDbContext _context;
        //private IdentityDbContext _identityContext;
        private ILogger<SeedData> _logger;

        //public SeedData(MealDbContext context, IdentityDbContext identityDbContext, ILogger<SeedData> logger)
        public SeedData(MealDbContext context, ILogger<SeedData> logger)
        {
            _context = context;
            //_identityContext = identityDbContext;
            _logger = logger;
        }

        public void EnsurePopulated(bool dropExisting)
        {
            if (dropExisting)
            {
                _context.Database.Migrate();
            }
            if (_context.Students.Count() == 0 && _context.Products.Count() == 0 && _context.Packages.Count() == 0 && _context.Canteens.Count() == 0 && _context.Employees.Count() == 0)
            {
                _logger.LogInformation("Preparing to seed voedselverspilling database");

                //Studenten seeden
                _context.Students.AddRange(DbSeeders.SeedStudents());

                //Producten seeden
                var products = DbSeeders.SeedProducts();
                _context.Products.AddRange(products);

                //Kantines seeden
                var canteens = DbSeeders.SeedCanteens();
                _context.Canteens.AddRange(canteens);

                //Employee's seeden
                _context.Employees.AddRange(DbSeeders.SeedEmpoyees(canteens));

                //Pakketten seeden
                _context.Packages.AddRange(DbSeeders.SeedPackages(products, canteens));

                _context.SaveChanges();
                _logger.LogInformation("Database voedselverspilling seeded");
            }

            else
            {
                _logger.LogInformation("Database voedselverspilling not seeded");
            }
        }

        //public void EnsurePopulatedIdentity(bool dropExisting)
        //{
        //    if (dropExisting)
        //    {
        //        _identityContext.Database.Migrate();
        //    }
        //    if (_identityContext.Users.Count() == 0)
        //    {
        //        _logger.LogInformation("Preparing to seed identity database");

        //        //Users seeden
        //        _identityContext.Users.AddRange(DbSeeders.SeedUsers());

        //        _identityContext.SaveChanges();
        //        _logger.LogInformation("Database identity seeded");
        //    }

        //    else
        //    {
        //        _logger.LogInformation("Database identity not seeded");
        //    }
        //}
    }
}
