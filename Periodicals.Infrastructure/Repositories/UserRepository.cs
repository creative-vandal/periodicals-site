﻿using System.Data.Entity;
using System.Linq;
using Periodicals.Core.Identity;
using Periodicals.Core.Interfaces;
using Periodicals.Infrastructure.Data;

namespace Periodicals.Infrastructure.Repositories
{
    public class UserRepository: IUserRepository
    {

        public ApplicationUser GetById(string userId)
        {
            using (var db = new PeriodicalDbContext())
            {
                var result = (from user in db.Users where user.Id == userId select user)
                    .Include(e => e.Subscription).Include(e=>e.Roles)
                    .FirstOrDefault();

                return result;
            }
        }

        public bool TopUp(string userId, float amount)
        {
            using (var db = new PeriodicalDbContext())
            {
                var user = (from user1 in db.Users where user1.Id == userId select user1).FirstOrDefault();
                if(user!=null) user.Credit += amount;
                db.SaveChanges();
                return true;
            }
        }
    }
}
