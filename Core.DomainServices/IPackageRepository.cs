using Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainServices
{
    public interface IPackageRepository : IRepository<Package>
    {
        public List<Package> GetByCanteenSortedByDate(Canteen canteen);
        public List<Package> GetOtherByCanteenSortedByDate(Canteen canteen);
        public IEnumerable<Package> GetAllPackagesGraphQL();

    }
}
