using eShop.DataAccess.EntityFramework.Context;
using eShop.Domain;

namespace eShop.DataAccess.EntityFramework
{
    public class EFProductRepository : EFRepository<Product>
    {
        public EFProductRepository(OnlineShopContext context) : base(context)
        {
        }
    }
}