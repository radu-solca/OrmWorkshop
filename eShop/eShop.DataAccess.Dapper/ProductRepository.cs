using System;
using System.Collections.Generic;
using System.Data;
using Dapper;
using eShop.Domain;

namespace eShop.DataAccess.Dapper
{
    public class ProductRepository: DapperRepository<Product>
    {
        public ProductRepository() : base("[Product]")
        {
        }

        public override ICollection<Product> GetAll()
        {
            return GetAllDefault();
        }

        protected override void Insert(Product product, IDbConnection connection)
        {
            var sql = @"INSERT INTO [Product]
           ([Id],[Name],[Description],[Price])
     VALUES
           (@Id, @Name, @Description, @Price)";

            connection.Execute(sql, product);
        }

        protected override void Update(Product entity, IDbConnection connection)
        {
            var sql = @"update [Product]
                            set [Name]=@Name
                            where [Id]=@Id";
            connection.Execute(sql, entity);
        }

        public void ExecuteProcedure(Product product)
        {
            using (IDbConnection cn = Connection)
            {
                cn.Open();
                var procedureName = "CreatePromo";
                var procedureParameters = new {Percent = 10};
                cn.Execute(procedureName, procedureParameters, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
