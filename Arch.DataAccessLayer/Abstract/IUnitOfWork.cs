using System.Threading.Tasks;

namespace Arch.DataAccessLayer.Abstract
{
    public interface IUnitOfWork
    {
        Task CommitAsync();
        void Commit();
    }
}
