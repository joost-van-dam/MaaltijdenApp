using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainServices
{
    public interface IRepository<T>
    {
        public List<T> Get();

        public T GetById(int id);

        public void Remove(int id);

        public void Create(T entity);
        public void Update(T entity);

    }
}
