using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Seeders
{
    public interface ISeedData
    {
        void EnsurePopulated(bool dropExisting = false);

        //void EnsurePopulatedIdentity(bool dropExisting = false);
    }
}
