﻿using System;
using System.Collections.Generic;
using eShop.Domain;
using eShop.Domain.Repositories;

namespace eShop.DataAccess.EntityFramework
{
    public class EFRepository<TEntity> : IRepository<TEntity> where TEntity : Entity
    {
        public void Create(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public ICollection<TEntity> Find(Func<TEntity, bool> filter)
        {
            throw new NotImplementedException();
        }

        public ICollection<TEntity> GetAll()
        {
            throw new NotImplementedException();
        }

        public TEntity GetbyId(Guid id)
        {
            throw new NotImplementedException();
        }

        public void Update(TEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
