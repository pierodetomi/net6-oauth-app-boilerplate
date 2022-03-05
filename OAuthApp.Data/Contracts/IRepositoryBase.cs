using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using System.Linq.Expressions;

namespace OAuthApp.Data.Contracts
{
    public interface IRepositoryBase<TEntity> where TEntity : class, new()
    {
        void Add(TEntity entity);
        void Attach(TEntity entity);
        void Commit();
        void Delete(Expression<Func<TEntity, bool>> predicate);
        void Delete(TEntity entity);
        void DeleteById(Guid id);
        void DeleteById(long id);
        void DeleteById(string id);
        void DeleteByKey(object id);
        IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);
        IQueryable<TEntity> TableAsNoTracking();
        DbSet<TEntity> GetAll();
        TEntity GetById(Guid id);
        TEntity GetById(int id);
        TEntity GetById(long id);
        TEntity GetById(string id);
        TEntity GetByKey(object key);
        IDbContextTransaction Transaction(IsolationLevel? isolationLevel = null);
        TEntity TryFindOne(Expression<Func<TEntity, bool>> predicate);
        void Update(TEntity entity);
    }
}