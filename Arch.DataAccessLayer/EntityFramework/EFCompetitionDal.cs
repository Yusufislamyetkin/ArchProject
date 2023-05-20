using Arch.DataAccessLayer.Abstract;
using Arch.DataAccessLayer.Concrete.Repositories;
using Arch.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arch.DataAccessLayer.EntityFramework
{
    internal class EFCompetitionDal : GenericRepository<Competition>, ICompetitionDal
    {
        public EFCompetitionDal(ArchDbContext context) : base(context)
        {
        }
    }
}
