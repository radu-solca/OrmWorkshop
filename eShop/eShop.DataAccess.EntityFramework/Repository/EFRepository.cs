using System;
using System.Collections.Generic;
using System.Linq;
using eShop.DataAccess.EntityFramework.Context;
using eShop.Domain;
using eShop.Domain.Repositories;
using EnsureThat;

namespace eShop.DataAccess.EntityFramework
{
    public abstract class EFRepository<TEntity> : IRepository<TEntity> where TEntity : Entity, new()
    {
        protected readonly OnlineShopContext Context;

        protected EFRepository(OnlineShopContext context)
        {
            EnsureArg.IsNotNull(context);
            this.Context = context;
        }

        public virtual TEntity GetById(Guid id)
        {
            return Context.Set<TEntity>().Find(id);
        }

        public virtual ICollection<TEntity> GetAll()
        {
            return Context.Set<TEntity>().ToList();
        }

        public virtual ICollection<TEntity> Find(Func<TEntity, bool> filter)
        {
            return Context.Set<TEntity>().Where(filter).ToList();
        }

        public virtual void Create(TEntity entity)
        {
            Context.Set<TEntity>().Add(entity);
            Context.SaveChanges();
        }

        public virtual void Update(TEntity entity)
        {
            Context.Set<TEntity>().Update(entity);
            Context.SaveChanges();
        }

        public virtual void Delete(Guid id)
        {
            TEntity entity = new TEntity{Id = id};
            Context.Set<TEntity>().Remove(entity);
            Context.SaveChanges();
        }
    }
}
