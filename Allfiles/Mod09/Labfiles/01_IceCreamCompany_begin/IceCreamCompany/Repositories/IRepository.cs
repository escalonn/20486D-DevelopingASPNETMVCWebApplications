using IceCreamCompany.Models;
using System.Collections.Generic;

namespace IceCreamCompany.Repositories
{
    public interface IRepository
    {
        IEnumerable<IceCream> GetIceCreamFlavors();
        IceCream GetIceCreamFlavorById(int id);
        void BuyIceCreamFlavor(Customer customer);
    }
}
