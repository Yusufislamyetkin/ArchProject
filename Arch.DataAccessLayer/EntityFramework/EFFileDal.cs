using Arch.DataAccessLayer.Abstract;
using Arch.DataAccessLayer.Concrete.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using File = Arch.EntityLayer.Entities.File;

namespace Arch.DataAccessLayer.EntityFramework
{
    internal class EFFileDal : GenericRepository<File>, IFileDal
    {
        public EFFileDal(ArchDbContext context) : base(context)
        {
        }
    }
}
