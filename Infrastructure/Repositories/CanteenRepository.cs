using Core.Domain;
using Core.DomainServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class CanteenRepository : ICanteenRepository
    {

        private readonly MealDbContext _context;

        public CanteenRepository(MealDbContext context)
        {
            _context = context;
        }
        public void Create(Canteen entity)
        {
            throw new NotImplementedException();
        }

        public List<Canteen> Get()
        {
            return _context.Canteens.ToList();
        }

        public Canteen GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Remove(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(Canteen entity)
        {
            throw new NotImplementedException();
        }
    }
}
