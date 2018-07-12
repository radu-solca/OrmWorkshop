using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Dapper;
using eShop.Domain;

namespace eShop.DataAccess.Dapper
{
    public class CustomerRepository : DapperRepository<Customer>
    {
        public CustomerRepository(string tableName) : base(tableName)
        {
        }

        public override ICollection<Customer> GetAll()
        {
            return base.GetAllDefault();
        }

        //StronglyTyped
        public override void Insert(Customer customer, IDbConnection connection)
        {
            var sql = @"INSERT INTO [Customer] ([Id],[FirstName],[LastName],[Age])
 values (@Id, @FirstName, @LastName, @Age)";
            connection.Execute(sql, customer);
        }

         public override void Update(Customer customer, IDbConnection connection)
        {
            var sql = @"Update Customer 
                Set FirstName = @FirstName
                Where Id = @Id";
            var parameters = new[]
            {
                new {Id = customer.Id, FirstName = "NumeAnonymus"}
            };
            connection.Execute(sql,parameters);
        }


        public void InsertStronglyTyped(Customer customer, IDbConnection connection)
        {
            var sql = @"INSERT INTO [Customer] ([Id],[FirstName],[LastName],[Age])
 values (@Id, @FirstName, @LastName, @Age)";
            connection.Execute(sql, customer);
        }

        public void InsertWithAnonymusParameters(Customer customer, IDbConnection connection)
        {
            var sql = @"INSERT INTO [Customer] ([Id],[FirstName],[LastName],[Age])
 values (@Id, @FirstName, @LastName, @Age)";
            var parameters = new[]
            {
                new {Id = 2, FirstName = "NumeAnonymus", LastName = "adressAnonymus", Age = 7}
            };
            connection.Execute(sql,parameters);
        }

        public void CreateTable()
        {
            using (IDbConnection con = Connection)
            {
                con.Open();
                var sql = @"create table [Customer] (
 [Id] uniqueidentifier primary key,
 [FirstName] varchar(100) not null,
 [LastName] varchar(100) not null,
 [Age] int not null)";
                con.Execute(sql);
                con.Close();
            }

        }


    }
}
