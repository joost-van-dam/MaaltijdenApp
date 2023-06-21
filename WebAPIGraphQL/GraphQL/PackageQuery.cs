using Core.Domain;
using Core.DomainServices;
using HotChocolate;

namespace WebAPIGraphQL.GraphQL
{
    public class PackageQuery
    {
        [UseFiltering]
        [UseSorting]
        public IEnumerable<Package> GetAllPackages([Service] IPackageRepository packageRepository) 
        {
            return packageRepository.GetAllPackagesGraphQL();
        }

        public Package GetPackage([Service] IPackageRepository packageRepository, int id)
        {
            return packageRepository.GetById(id);
        }

    }
}
