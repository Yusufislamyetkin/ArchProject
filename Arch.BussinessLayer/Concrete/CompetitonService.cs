using Arch.BussinessLayer.Abstract;
using Arch.DataAccessLayer.Abstract;
using Arch.EntityLayer.Entities;

namespace Arch.BussinessLayer.Concrete
{
    public class CompetitonService : Service<Competition>, ICompetitonService
    {
        public CompetitonService(IRepository<Competition> repository, IUnitOfWork unitOfWork) : base(repository, unitOfWork)
        {
        }
    }
}
