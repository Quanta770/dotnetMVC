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
    public class UsersRepository : Repository<ApplicationUser>, IUsersRepository
    {
        private ApplicationDBContext _dbContext;
        public UsersRepository(ApplicationDBContext db) : base(db)
        {
            _dbContext = db;
        }

        public void Update(ApplicationUser user)
        {
            throw new NotImplementedException();
        }
    }


}
