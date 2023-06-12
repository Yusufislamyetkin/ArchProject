using Arch.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arch.BussinessLayer.Abstract
{
    public interface IFileService : IService<ProjectFilePath>
    {
        Task<ProjectFilePath> CreateFile(ProjectFilePath projectFilePath);
        Task<List<ProjectFilePath>> GetByCompId(int Id);
    }
}
