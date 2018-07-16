using System;
using System.Collections.Generic;
using System.Linq;
using eShop.DataAccess.EntityFramework.Context;
using eShop.Domain;
using Microsoft.EntityFrameworkCore;

namespace eShop.DataAccess.EntityFramework
{
    public class EFOrderRepository : EFRepository<Order>
    {
        public EFOrderRepository(OnlineShopContext context) : base(context)
        {
        }

        public override ICollection<Order> GetAll()
        {
            return Context
                .Set<Order>()
                .Include(o => o.Customer)
                .Include(o => o.Items)
                .ToList();
        }

        public override Order GetById(Guid id)
        {
            return Context
                .Set<Order>()
                .Include(o => o.Customer)
                .Include(o => o.Items)
                .FirstOrDefault(o => o.Id == id);
        }

        public override ICollection<Order> Find(Func<Order, bool> filter)
        {
            return Context
                .Set<Order>()
                .Include(o => o.Customer)
                .Where(filter)
                .ToList();
        }
    }
}