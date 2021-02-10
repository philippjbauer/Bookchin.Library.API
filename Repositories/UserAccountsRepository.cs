using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Bookchin.Library.API.Data.Contexts;
using Bookchin.Library.API.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Bookchin.Library.API.Repositories
{
    public class UserAccountsRepository : CrudRepositoryBase<UserAccount>
    {
        public UserAccountsRepository(ILogger<UserAccountsRepository> logger, AppDbContext db)
            : base(logger, db) { }

        public override List<UserAccount> List(Expression<Func<UserAccount, bool>> predicate = null)
        {
            List<UserAccount> userAccounts = this.DbSet
                .TagWith($"Get list of records of type UserAccount.")
                .Where(predicate ?? (userAccount => userAccount.IsActive == true))
                .Include(userAccount => userAccount.Address)
                .ToList();

            return userAccounts;
        }
    }
}