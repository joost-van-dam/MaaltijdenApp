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
    public class StudentRepository : IStudentRepository
    {
        private readonly MealDbContext _context;

        public StudentRepository(MealDbContext context)
        {
            _context = context;
        }

        public void Create(Student entity)
        {
            throw new NotImplementedException();
        }

        public List<Student> Get()
        {
            throw new NotImplementedException();
        }

        public Student GetByEmailAddress(string emailAddress)
        {
            return _context.Students.Where(e => e.EmailAddress == emailAddress).Include(s => s.Packages).FirstOrDefault();
        }

        public Student GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Remove(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(Student entity)
        {
            throw new NotImplementedException();
        }
    }
}
