using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Bookchin.Library.API.Data.Contexts;
using Bookchin.Library.API.Exceptions;
using Bookchin.Library.API.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Bookchin.Library.API.Repositories
{
    // Todo:
    // [✓] Implement sync interface
    // [ ] Implement async interface
    
    /// <summary>
    /// Base for simple CRUD functionality on the database.
    /// </summary>
    /// <typeparam name="TEntity">Bookchin.Library.API.Data.Models Type</typeparam>
    public abstract class CrudRepositoryBase<TEntity> : IModelRepository<TEntity>
        where TEntity : class, IDbModel
    {
        public DbSet<TEntity> DbSet => GetDbSet();

        protected readonly ILogger<CrudRepositoryBase<TEntity>> _logger;

        protected readonly AppDbContext _db;

        private readonly Type _recordType = typeof(TEntity);

        public CrudRepositoryBase(ILogger<CrudRepositoryBase<TEntity>> logger, AppDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        /// <summary>
        /// Returns a list of all records in the DBSet that 
        /// fulfill the conditions of an optional predicate.
        /// </summary>
        /// <param name="predicate">Filter expression</param>
        /// <returns>A (optionally filtered) list of records.</returns>
        public virtual List<TEntity> List(Expression<Func<TEntity, bool>> predicate = null)
        {
            List<TEntity> records = this.DbSet
                .TagWith<TEntity>($"Get list of records of type {_recordType.Name}.")
                .Where(predicate)
                .ToList();
            
            return records;
        }

        /// <summary>
        /// Returns a single record of the DBSet that matches
        /// the given record id.
        /// </summary>
        /// <param name="id">Record id</param>
        /// <returns>A single record.</returns>
        /// <exception cref="Bookchin.Library.API.Exceptions.ModelRecordNotFoundException">
        /// Thrown when the given id parameter finds no corresponding record.
        /// </exception>
        public virtual TEntity Read(Guid id)
        {
            TEntity record = this.DbSet
                .TagWith<TEntity>($"Get single record of type \"{_recordType.Name}\" for Id \"{id}\".")
                .SingleOrDefault(record => record.Id == id);

            if (record == null)
            {
                throw new ModelRecordNotFoundException(
                    $"Could not find record of type \"{_recordType.Name}\" for Id \"{id}\"."
                );
            }

            return record;
        }

        public virtual TEntity Add(TEntity record)
        {
            record.Verify();

            this.DbSet.Add(record);
            _db.SaveChanges();

            return record;
        }

        public virtual List<TEntity> AddRange(List<TEntity> records)
        {
            foreach (TEntity record in records)
            {
                record.Verify();
            }

            this.DbSet.AddRange(records);
            _db.SaveChanges();

            return records;
        }

        public virtual TEntity Update(TEntity record)
        {
            if (record.IsPersisted == false)
            {
                throw new ModelRecordNotFoundException(
                    $"Trying to update a model which is not persisted."
                );
            }

            record.Verify();

            this.DbSet.Update(record);
            _db.SaveChanges();

            return record;
        }

        public virtual void Delete(Guid id)
        {
            TEntity record = Read(id);
            Delete(record);
        }

        public virtual void Delete(TEntity record)
        {
            if (record.IsPersisted == false)
            {
                throw new ModelRecordNotFoundException(
                    $"Trying to remove a model which is not persisted."
                );
            }

            this.DbSet.Remove(record);
            _db.SaveChanges();
        }
        
        /// <summary>
        /// Exposes the underlying DbSet for
        /// complex repository interactions.
        /// </summary>
        /// <returns>A DbSet of type T</returns>
        private DbSet<TEntity> GetDbSet()
        {
            return _db.Set<TEntity>(_recordType.FullName);
        }
    }
}