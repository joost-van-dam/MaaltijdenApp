using Core.Domain;
using Core.DomainServices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly MealDbContext _context;

        public ProductRepository(MealDbContext context)
        {
            _context = context;
        }

        public void Create(Product entity)
        {
            _context.Products.Add(entity);
            _context.SaveChangesAsync();
        }

        public List<Product> Get()
        {
            return _context.Products.Include(p => p.Packages).ThenInclude(x => x.Reservation).ToList();
        }

        public List<Product> Get(List<int> ids)
        {
            List<Product> products = new List<Product>();

            foreach (int id in ids)
            {
                products.Add(GetById(id));
            }
            return products;

            //return ids.ForEach(id => _context.Products.Where(p => p.Id == id)).ToList();
        }

        public Product GetById(int id)
        {
            return _context.Products.Where(p => p.Id == id).Include(p => p.Packages).ThenInclude(x => x.Reservation).First();
        }

        public void Remove(int id)
        {
            _context.Products.Remove(GetById(id));
            _context.SaveChangesAsync();
        }

        public void Update(Product entity)
        {
            var oldProduct = _context.Products.Include(p => p.Packages).ThenInclude(x => x.Reservation).FirstOrDefault(p => p.Id == entity.Id);
            if (oldProduct != null)
            {
                oldProduct.Name = entity.Name;
                oldProduct.ContainsAlcohol = entity.ContainsAlcohol;

                if (entity.ImageFormat != null)
                {
                    oldProduct.ImageFormat = entity.ImageFormat;
                    oldProduct.ImageData = entity.ImageData;
                }

                _context.Products.Update(oldProduct);
            }

            _context.SaveChanges();
        }
    }
}
