using Arch.DataAccessLayer.Concrete.Repositories;

namespace Arch.DataAccessLayer.Abstract
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ArchDbContext _context;

        public UnitOfWork(ArchDbContext context)
        {
            _context = context;
        }

        public void Commit()
        {
            _context.SaveChanges();
        }

        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
