using eShop.DataAccess.EntityFramework.Context;
using eShop.Domain;

namespace eShop.DataAccess.EntityFramework
{
    public class EFCustomerRepository : EFRepository<Customer>
    {
        public EFCustomerRepository(OnlineShopContext context) : base(context)
        {
        }
    }
}