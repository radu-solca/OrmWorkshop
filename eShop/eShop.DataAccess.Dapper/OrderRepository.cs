﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using eShop.Domain;

namespace eShop.DataAccess.Dapper
{
    public class OrderRepository : DapperRepository<Order>
    {
        public OrderRepository(string tableName) : base(tableName)
        {
        }

        //Explain how transaction works
        public override void Insert(Order order, IDbConnection cn)
        {
            var sqlInsertProduct = @"insert into [Product] ([Id], [Description], [Price])
 values (@Id, @Description, @Price)";

            var sqlInsertOrder = @"insert into [Order] ([Id], [CustomerId], [Amount], [PlacedAt], [DeliveredAt])
values (@Id, @CustomerId, @Amount, @PlacedAt, @DeliveredAt)";

            var sqlInsertOrderItem = @"insert into [OrderItem] ([Id], [OrderId], [ProductId], [Quantity])
values (@Id, @OrderId, @ProductId, @Quantity)";

            using (var transaction = cn.BeginTransaction())
            {
                cn.Execute(sqlInsertProduct, order.Items[0].Product, transaction: transaction);
                cn.Execute(sqlInsertOrder, order, transaction: transaction);
                cn.Execute(sqlInsertOrderItem, order.Items[0], transaction: transaction);

                transaction.Commit();
            }
        }

        public override ICollection<Order> GetAll()
        {
            return GetAllMultiMapping();
        }

        //Multi mapping with one to many relation
        // Maps single row to several objects
        private List<Order> GetAllMultiMapping()
        {
            using (IDbConnection cn = Connection)
            {
                cn.Open();

                var sql = @"select * from [Order] fo join [OrderItem] oi on fo.[Id] = oi.[OrderId]";

                var orderDictionary = new Dictionary<Guid, Order>();
                var result = cn.Query<Order, OrderItem, Order>(sql,
                    (order, orderItem) =>
                    {
                        Order fullOrder;

                        if (!orderDictionary.TryGetValue(order.Id, out fullOrder))
                        {
                            // add parrent in dictionary if not exists
                            fullOrder = order;
                            fullOrder.Items = new List<OrderItem>();
                            orderDictionary.Add(fullOrder.Id, fullOrder);
                        }

                        //Add children to parent
                        fullOrder.Items.Add(orderItem);
                        return fullOrder;
                    },
                    splitOn: "OrderId");
                return result.ToList();
            }
        }

        //Multitype
        // Processes multiple grids with a single query
        private List<Order> GetAllMultiType()
        {
            using (IDbConnection cn = Connection)
            {
                cn.Open();

                var sql = @"select * from FullOrder;
                        select * from OrderItem";

                var orderDictionary = new Dictionary<int, Order>();
                using (var multipleQueryResult = cn.QueryMultiple(sql))
                {
                    var orders = multipleQueryResult.Read<Order>().ToList();
                    var ordersItems = multipleQueryResult.Read<OrderItem>().ToList();
                    foreach (var item in ordersItems)
                    {
                        orders.First(o => o.Id == item.Id).Items.Add(item);
                    }

                    return orders;
                }
            }
        }

        public override void Update(Order entity, IDbConnection connection)
        {
            throw new NotImplementedException();
        }

        public void CreateTables()
        {
            using (IDbConnection con = Connection)
            {
                con.Open();
                var sql = @"create table [Order] (
                 [Id] uniqueidentifier primary key ,
                 [CustomerId] uniqueidentifier not null,
                 [PlacedAt] datetime2(7) not null,
				 [DeliveredAt] datetime2(7) null,
                 [Amount] decimal(6,2),
                  CONSTRAINT fk_order_customer_id FOREIGN KEY ([CustomerId]) REFERENCES Customer (Id));

create table [Product] (
                 [Id] uniqueidentifier primary key ,
                 [Description] varchar(100) not null,
                 [Price] decimal(6,2));

create table [OrderItem] (
                 [Id] uniqueidentifier primary key ,
                 [OrderId] uniqueidentifier not null,
                 [ProductId] uniqueidentifier not null,
                 [Quantity] int not null,
                  CONSTRAINT fk_orderItem_order_id FOREIGN KEY ([OrderId]) REFERENCES [Order] ([Id]),
CONSTRAINT fk_orderItem_product_id FOREIGN KEY ([ProductId]) REFERENCES [Product] ([Id]));";
                con.Execute(sql);
                con.Close();
            }

        }
    }
}
