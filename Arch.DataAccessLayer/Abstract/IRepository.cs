using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Arch.DataAccessLayer.Abstract
{
    public interface IRepository<T> where T : class
    {
        T Get(Expression<Func<T, bool>> filter);
        List<T> List();
        List<T> WhrList(Expression<Func<T, bool>> filter);
        void Insert(T entity);
        void Update(T entity);
        void UpdateNew(T entity, int id);
        void Delete(T entity);
    }
}
