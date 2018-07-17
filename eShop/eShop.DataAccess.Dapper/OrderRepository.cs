using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using eShop.Domain;

namespace eShop.DataAccess.Dapper
{
    public class OrderRepository : DapperRepository<Order>
    {
        public OrderRepository() : base("[Order]")
        {
        }

        //The purpose of this method is to show how to work with transactions with Dapper
        // In a real life scenario we won't insert a product when we place a worder
        protected override void Insert(Order order, IDbConnection cn)
        {
            var sqlInsertProduct = @"insert into [Product] ([Id], [Description], [Price])
 values (@Id, @Description, @Price)";

            var sqlInsertOrder = @"insert into [Order] ([Id], [CustomerId], [Amount], [PlacedAt], [DeliveredAt])
values (@Id, @CustomerId, @Amount, @PlacedAt, @DeliveredAt)";

            var sqlInsertOrderItem = @"insert into [OrderItem] ([Id], [OrderId], [ProductId], [Quantity])
values (@Id, @OrderId, @ProductId, @Quantity)";

            //Start a transaction
            using (var transaction = cn.BeginTransaction())
            {
                //send transaction as parameter to each execute insert
                cn.Execute(sqlInsertProduct, order.Items[0].Product, transaction: transaction);
                var orderParameters = new
                {
                    Id = order.Id,
                    CustomerId = order.CustomerId,
                    PlacedAt = order.PlacedAt,
                    DeliveredAt = order.DeliveredAt,
                    Amount = order.TotalPrice
                };
                cn.Execute(sqlInsertOrder, orderParameters, transaction: transaction);
                cn.Execute(sqlInsertOrderItem, order.Items[0], transaction: transaction);

                //!!! commit transaction. If you don't commit it, the objects won't be inserted in database;
                transaction.Commit();
            }
        }

        public override ICollection<Order> GetAll()
        {
            return GetAllMultiMapping();
            //return GetAllMultiType();
        }

        //Multi mapping with one to many relation
        // Maps single row to several objects
        private List<Order> GetAllMultiMapping()
        {
            using (IDbConnection cn = Connection)
            {
                cn.Open();

                var sql = @"select fo.[Id],fo.[CustomerId],fo.[PlacedAt],fo.[DeliveredAt],fo.[Amount] as TotalPrice,
oi.[OrderId],oi.[Id],oi.[ProductId],oi.[Quantity] 
from [Order] fo join [OrderItem] oi on fo.[Id] = oi.[OrderId]";

                var orderDictionary = new Dictionary<Guid, Order>();

                //dapper will interpret that 
                //a returned row from sql result is a combination of Order and OrderItem
                //and it returns an Order(third parameter)
                var result = cn.Query<Order, OrderItem, Order>(sql,
                    //mapping function 
                    //sql result order and orderItem will be mapped in dictionary 
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

                var sql = @"select fo.[Id],fo.[CustomerId],fo.[PlacedAt],fo.[DeliveredAt],fo.[Amount] as TotalPrice from [Order] fo;
                        select * from OrderItem";

                var orderDictionary = new Dictionary<int, Order>();
                using (var multipleQueryResult = cn.QueryMultiple(sql))
                {
                    var orders = multipleQueryResult.Read<Order>().ToList();
                    var ordersItems = multipleQueryResult.Read<OrderItem>().ToList();
                    foreach (var item in ordersItems)
                    {
                        orders.First(o => o.Id == item.OrderId).Items.Add(item);
                    }

                    return orders;
                }
            }
        }

        protected override void Update(Order entity, IDbConnection connection)
        {
            throw new NotImplementedException();
        }

    }
}
