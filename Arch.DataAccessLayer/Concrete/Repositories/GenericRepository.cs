using Arch.DataAccessLayer.Abstract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Arch.DataAccessLayer.Concrete.Repositories
{
    public class GenericRepository<T> : IRepository<T> where T : class
    {
        private readonly ArchDbContext _context;
        private readonly DbSet<T> _object;

        public GenericRepository(ArchDbContext context)
        {
            _context = context;
            _object = context.Set<T>();
        }

        public void Delete(T entity)
        {
            _object.Remove(entity);
            _context.SaveChanges();
        }

        public T Get(Expression<Func<T, bool>> filter)
        {
            return _object.SingleOrDefault(filter);
        }

        public void Insert(T entity)
        {
            _object.Add(entity);
            _context.SaveChanges();
        }

        public List<T> List()
        {
            return _object.ToList();
        }

        public void Update(T entity)
        {
            _object.Update(entity);
            _context.SaveChanges();
        }

        public void UpdateNew(T entity, int id)
        {
            var existingEntity = _object.Find(id);
            if (existingEntity != null)
            {
                _context.Entry(existingEntity).CurrentValues.SetValues(entity);
                _context.SaveChanges();
            }
        }

        public List<T> WhrList(Expression<Func<T, bool>> filter)
        {
            return _object.Where(filter).ToList();
        }
    }
}
