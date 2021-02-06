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
    public class IndividualsRepository : CrudRepositoryBase<Individual>
    {
        public IndividualsRepository(ILogger<IndividualsRepository> logger, AppDbContext db)
            : base(logger, db) { }

        public override List<Individual> List(Expression<Func<Individual, bool>> predicate = null)
        {
            List<Individual> individuals = this.DbSet
                .TagWith($"Get list of records of type Individual.")
                .Where(predicate ?? (individual => individual.IsActive == true))
                .Include(individual => individual.Address)
                .ToList();

            return individuals;
        }

        public Individual Create(IndividualViewModel vm)
        {
            var individual = new Individual(vm);

            return Add(individual);
        }

        public Individual Update(Guid id, IndividualViewModel vm)
        {
            Individual individual = Read(id);
            
            individual.UpdateFromVm(vm);

            return Update(individual);
        }
    }
}