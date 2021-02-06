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
    public class UsersRepository : CrudRepositoryBase<User>
    {
        public UsersRepository(ILogger<UsersRepository> logger, AppDbContext db)
            : base(logger, db) { }

        public override List<User> List(Expression<Func<User, bool>> predicate = null)
        {
            List<User> users = this.DbSet
                .TagWith($"Get list of records of type User.")
                .Where(predicate ?? (user => user.IsActive == true))
                .Include(user => user.Address)
                .ToList();

            return users;
        }
    }
}