using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arch.EntityLayer.Entities;
using File = Arch.EntityLayer.Entities.File;

namespace Arch.DataAccessLayer.Abstract
{
    internal interface IFileDal : IRepository<File>
    {
    }
}
