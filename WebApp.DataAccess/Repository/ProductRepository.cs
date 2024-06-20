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
            var productFrmDb = _dbContext.Product.FirstOrDefault(x => x.Id == obj.Id);
            if (productFrmDb != null)
            {
                productFrmDb.Title = obj.Title;
                productFrmDb.ISBN = obj.ISBN;
                productFrmDb.ListPrice = obj.ListPrice;
                productFrmDb.Publisher = obj.Publisher;
                productFrmDb.Description = obj.Description;
                productFrmDb.CategoryId = obj.CategoryId;
                productFrmDb.Author = obj.Author;
            }
            if (obj.ImageURL != null)
            {
                productFrmDb.ImageURL = obj.ImageURL;
            }
        }
    }
}
