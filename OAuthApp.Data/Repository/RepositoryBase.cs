using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using OAuthApp.Data.Contracts;
using OAuthApp.Data.Entities;
using System.Data;
using System.Linq.Expressions;

namespace OAuthApp.Data.Repository
{
    public abstract class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : class, new()
    {
        protected readonly AppDbContext _db;

        public RepositoryBase(AppDbContext db)
        {
            _db = db;
        }

        public DbSet<TEntity> GetAll()
        {
            return _db.Set<TEntity>();
        }

        public IQueryable<TEntity> TableAsNoTracking()
        {
            return _db.Set<TEntity>().AsNoTracking();
        }

        public void Add(TEntity entity)
        {
            _db.Add(entity);
        }

        public void DeleteById(Guid id)
        {
            DeleteByIdCore(id);
        }

        public void DeleteById(long id)
        {
            DeleteByIdCore(id);
        }

        public void DeleteById(string id)
        {
            DeleteByIdCore(id);
        }

        private void DeleteByIdCore(object id)
        {
            TEntity entity = GetByKey(id);
            _db.Remove(entity);
        }

        public void DeleteByKey(object id)
        {
            TEntity entity = GetByKey(id);

            _db.Remove(entity);
        }

        public void Delete(TEntity entity)
        {
            _db.Remove(entity);
        }

        public void Delete(Expression<Func<TEntity, bool>> predicate)
        {
            var entities = _db.Set<TEntity>().Where(predicate).ToList();
            entities.ForEach(entity => _db.Remove(entity));
        }

        public TEntity GetById(int id)
        {
            return GetByKey(id);
        }

        public TEntity GetById(long id)
        {
            return GetByKey(id);
        }

        public TEntity GetById(Guid id)
        {
            return GetByKey(id);
        }

        public TEntity GetById(string id)
        {
            return GetByKey(id);
        }

        public TEntity GetByKey(object key)
        {
            return _db.Set<TEntity>().Find(key);
        }

        public TEntity TryFindOne(Expression<Func<TEntity, bool>> predicate)
        {
            return _db.Set<TEntity>().SingleOrDefault(predicate);
        }

        public IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return _db.Set<TEntity>().Where(predicate);
        }

        public void Update(TEntity entity)
        {
            var dbEntry = _db.Entry(entity);
            // _db.Attach(dbEntry);
            dbEntry.State = EntityState.Modified;
            // _db.Update(entity);
        }

        public void Attach(TEntity entity)
        {
            var entry = _db.Attach(entity);

            var entityType = _db.Model.FindEntityType(typeof(TEntity));
            entityType.GetProperties().ToList().ForEach(property =>
            {
                if (property.IsKey())
                    return;

                entry.Property(property.Name).IsModified = true;
            });

            entry.State = EntityState.Modified;
        }

        public IDbContextTransaction Transaction(IsolationLevel? isolationLevel = null)
        {
            if (isolationLevel.HasValue) return _db.Database.BeginTransaction(isolationLevel.Value);
            else return _db.Database.BeginTransaction();
        }

        public void Commit()
        {
            _db.SaveChanges();
        }
    }
}