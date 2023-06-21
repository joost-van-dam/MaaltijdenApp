using Core.Domain;
using Core.DomainServices;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class PackageRepository : IPackageRepository
    {
        //best akelig dat ik deze in iedere Repository moet zetten
        //dit doe ik niet liever via de interface
        //maar die zit in een diepere schil
        private readonly MealDbContext _context;

        public PackageRepository(MealDbContext context)
        {
            _context = context;
        }

        public void Create(Package entity)
        {
            _context.Packages.Add(entity);
            _context.SaveChangesAsync();
        }

        public List<Package> Get()
        {
            return _context.Packages.Include(p => p.Reservation).Include(p => p.Canteen).ToList();
        }

        public IEnumerable<Package> GetAllPackagesGraphQL()
        {
            return _context.Packages.Include(p => p.Reservation).Include(p => p.Products).Include(p => p.Canteen).ToList();
        }

        public Package GetById(int id)
        {
            var package = _context.Packages.Where(x => x.Id == id).Include(p => p.Products).Include(p => p.Reservation).Include(q => q.Canteen).FirstOrDefault();
            
            if (package == null)
            {
                package = errorPackage();
            }

            return package;
        }

        private Package errorPackage()
        {
            return new Package
            {
                Name = "Dit pakket bestaat niet",
                Products = new List<Product>
                    {
                        new Product
                        {
                            Name = "Dit product bestaat niet"
                        }
                    },
                Canteen = new Canteen
                {
                    Name = "Deze kantine bestaat niet",
                    City = City.Breda,
                },
                Price = 0,
            };
        }

        public List<Package> GetByCanteenSortedByDate(Canteen canteen)
        {
            return _context.Packages.Where(p => p.Canteen == canteen).OrderBy(p => p.PickupMoment).Include(p => p.Reservation).Include(p => p.Canteen).ToList();
        }

        public List<Package> GetOtherByCanteenSortedByDate(Canteen canteen)
        {
            return _context.Packages.Where(p => p.Canteen != canteen).OrderBy(p => p.PickupMoment).Include(p => p.Reservation).Include(p => p.Canteen).ToList();
        }

        public void Remove(int id)
        {
            _context.Packages.Remove(GetById(id));
            _context.SaveChangesAsync();
        }

        //Misschien deze signature aanpassen als het al op entity gaat
        public void Update(Package entity)
        {
            var oldPackage = _context.Packages.Include(package => package.Products).FirstOrDefault(fp => fp.Id == entity.Id);
            if (oldPackage != null)
            {
                if (entity.Products != null)
                {
                    oldPackage.Products.Any(p => oldPackage.Products.Remove(p));
                    oldPackage.Products = entity.Products;
                }
                oldPackage.City = entity.City;
                oldPackage.Canteen = entity.Canteen;
                oldPackage.Name = entity.Name;
                oldPackage.Price = entity.Price;
                oldPackage.IsOver18 = entity.IsOver18;
                oldPackage.TypeOfMeal = entity.TypeOfMeal;
                oldPackage.PickupMoment = entity.PickupMoment;
                oldPackage.PickupClosingTime = entity.PickupClosingTime;
                if (entity.Reservation != null)
                {
                    oldPackage.Reservation = entity.Reservation;
                }

                _context.Packages.Update(oldPackage);
            }

            _context.SaveChanges();
        }
    }
}
