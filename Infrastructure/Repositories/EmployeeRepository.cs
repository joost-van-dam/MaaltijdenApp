using Core.Domain;
using Core.DomainServices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly MealDbContext _context;

        public EmployeeRepository(MealDbContext context)
        {
            _context = context;
        }

        public void Create(Employee entity)
        {
            throw new NotImplementedException();
        }

        public List<Employee> Get()
        {
            throw new NotImplementedException();
        }

       

        public Employee GetByEmailAddress(string emailAddress)
        {
            return _context.Employees.Where(e => e.EmailAddress == emailAddress).Include(e => e.Canteen).First();
        }

        public Employee GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Remove(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(Employee entity)
        {
            throw new NotImplementedException();
        }
    }
}
