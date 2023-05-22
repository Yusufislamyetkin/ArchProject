using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arch.EntityLayer.Entities;
using File = Arch.EntityLayer.Entities.File;

namespace Arch.DataAccessLayer.Abstract
{
    public interface IFileDal : IRepository<File>
    {
    }
}
