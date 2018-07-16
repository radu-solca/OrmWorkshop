using System;
using System.Collections.Generic;
using eShop.Domain;
using eShop.Domain.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace eShop.DataAccess.Dapper.Tests
{
    [TestClass]
    public class OrderTests
    {
        private IRepository<Order> _repository;

        [TestInitialize]
        public void Initialize()
        {
            _repository = new OrderRepository();
        }

        [TestMethod]
        public void InsertOrder()
        {
            /*INSERT INTO [dbo].[Customer] ([Id],[FirstName],[LastName],[Age])
VALUES ('49d1967c-2a10-49db-b797-db57eabfc4f8','Customer for','insert order',20)*/
            var orderId = Guid.NewGuid();
            var productId = Guid.NewGuid();

            var product = new Product
            {
                Id = productId,
                Description = "ProductDescription",
                Name = "ProductName",
                Price = 20
            };

            var orderItem = new OrderItem
            {
                Id = Guid.NewGuid(),
                OrderId = orderId,
                ProductId = productId,
                Quantity = 3,
                Product = product
            };

            var order = new Order
            {
                Id = orderId,
                TotalPrice = 100,
                CustomerId = new Guid("49d1967c-2a10-49db-b797-db57eabfc4f8"),
                PlacedAt = DateTime.Now,
                Items = new List<OrderItem>{orderItem}
            };
            _repository.Create(order);
        }

        [TestMethod]
        public void GetAll()
        {
            var result = _repository.GetAll();
        }
    }
}
