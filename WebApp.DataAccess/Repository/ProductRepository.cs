using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp.DataAccess.Data;
using WebApp.DataAccess.Repository.IRepository;
using WebApp.Models;

namespace WebApp.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private ApplicationDBContext _dbContext;
        public ProductRepository(ApplicationDBContext db) : base(db)
        {
            _dbContext = db;
        }
        public void Update(Product obj)
        {
            _dbContext.Update(obj);
        }
    }
}
