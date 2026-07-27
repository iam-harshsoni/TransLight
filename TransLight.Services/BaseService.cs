using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TransLight.DataAccess.Data;
using TransLight.Services.Interfaces;

namespace TransLight.Services
{
    public class BaseService<T> : IBaseService<T> where T : class
    {
        private readonly TransLightContext _db;
        internal DbSet<T> dbset;

        public BaseService(TransLightContext db)
        {
            _db = db;
            this.dbset = _db.Set<T>();
        }

        public void Add(T entity)
        {
            _db.Add(entity);
        }

        public T Get(Expression<Func<T, bool>> filter, string? includeProperties = null)
        {
            IQueryable<T> query = dbset;
            query = query.Where(filter);

            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includProp in includeProperties
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includProp);
                }
            }

            return query.FirstOrDefault();
        }
        public IQueryable<T> GetAll(string? includeProperties = null)
        {
            IQueryable<T> query = dbset;

            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includProp in includeProperties
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includProp);
                }
            }

            return query;
        }

        public void Remove(T entity)
        {
            dbset.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            dbset.RemoveRange(entities);
        }
        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
