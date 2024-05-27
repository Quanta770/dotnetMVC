using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WebApp.DataAccess.Data;
using WebApp.DataAccess.Repository.IRepository;

namespace WebApp.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDBContext _db;
        internal DbSet<T> dBSet; //represents the collection of all entities in the context, or that can be queried from the database, of a given type
        public Repository(ApplicationDBContext db)
        {
            _db = db;
            this.dBSet = _db.Set<T>();
            //when T= Category; _db.Category == dbSet
            //_db.Category.Add() == dbSet.Add()
        }
        public void Add(T entity)
        {
            // cannot call generic type T here: _db.T
            dBSet.Add(entity);
        }

        
        public T Get(Expression<Func<T, bool>> filter, string? includeProperties = null)
        {
            IQueryable<T> query = dBSet;
            // same as _db.Catgory.Where(x=>x.Id==id).FirstOrDefault()
            query = query.Where(filter); //_db.Category.Where(x=>x...)
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var property in includeProperties
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(property);

                }
            }
            return query.FirstOrDefault(); //_db.Category.Where(x=>x...).FirstOrDefault()
        }

        public IEnumerable<T> GetAll(string? includeProperties=null)
        {
           IQueryable<T> query = dBSet;
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach(var property in includeProperties
                    .Split(new char[] {','},StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(property);
                    
                }
            }
            return query.ToList();
        }

     

        public void Remove(T entity)
        {
            dBSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entity)
        {
            dBSet.RemoveRange(entity);
        }
    }
}
