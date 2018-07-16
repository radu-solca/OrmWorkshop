using System;
using System.Collections.Generic;
using System.Linq;
using eShop.DataAccess.EntityFramework.Context;
using eShop.Domain;
using Microsoft.EntityFrameworkCore;

namespace eShop.DataAccess.EntityFramework
{
    public class EFOrderItemRepository : EFRepository<OrderItem>
    {
        public EFOrderItemRepository(OnlineShopContext context) : base(context)
        {
        }

        public override ICollection<OrderItem> GetAll()
        {
            return Context
                .Set<OrderItem>()
                .Include(o => o.Order)
                .Include(o => o.Product)
                .ToList();
        }

        public override OrderItem GetById(Guid id)
        {
            return Context
                .Set<OrderItem>()
                .Include(o => o.Order)
                .Include(o => o.Product)
                .FirstOrDefault(o => o.Id == id);
        }

        public override ICollection<OrderItem> Find(Func<OrderItem, bool> filter)
        {
            return Context
                .Set<OrderItem>()
                .Include(o => o.Order)
                .Include(o => o.Product)
                .Where(filter)
                .ToList();
        }
    }
}