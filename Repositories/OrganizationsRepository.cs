using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Bookchin.Library.API.Controllers.ViewModels;
using Bookchin.Library.API.Data.Contexts;
using Bookchin.Library.API.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Bookchin.Library.API.Repositories
{
    public class OrganizationsRepository : CrudRepositoryBase<Organization>
    {
        public OrganizationsRepository(ILogger<OrganizationsRepository> logger, AppDbContext db)
            : base(logger, db) { }

        public override List<Organization> List(Expression<Func<Organization, bool>> predicate = null)
        {
            List<Organization> organizations = this.DbSet
                .TagWith($"Get list of records of type Organization.")
                .Where(predicate ?? (organization => organization.IsActive == true))
                .Include(organization => organization.Address)
                .ToList();

            return organizations;
        }

        public Organization Create(OrganizationViewModel vm)
        {
            var organization = new Organization(vm);

            return Add(organization);
        }

        public Organization Update(Guid id, OrganizationViewModel vm)
        {
            Organization organization = Read(id);

            organization.UpdateFromVm(vm);

            return Update(organization);
        }
    }
}