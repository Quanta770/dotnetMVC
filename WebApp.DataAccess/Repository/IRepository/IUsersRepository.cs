﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp.Models;

namespace WebApp.DataAccess.Repository.IRepository
{
    public interface IUsersRepository : IRepository<ApplicationUser>
    {
        void Update(ApplicationUser user);
    }
}
