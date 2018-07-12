﻿using System;
using System.Collections.Generic;

namespace eShop.Domain.Repositories
{
    public interface IRepository<TEntity> where TEntity : Entity
    {
        TEntity GetbyId(Guid id);
        ICollection<TEntity> GetAll();
        ICollection<TEntity> Find(Func<TEntity, bool> filter);

        void Create(TEntity entity);
        void Update(TEntity entity);
        void Delete(Guid id);
    }
}