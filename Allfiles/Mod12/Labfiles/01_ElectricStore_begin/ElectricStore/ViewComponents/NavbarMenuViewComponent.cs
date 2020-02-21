using System.Linq;
using ElectricStore.Data;
using Microsoft.AspNetCore.Mvc;

namespace ElectricStore.ViewComponents
{
    public class NavbarMenuViewComponent : ViewComponent
    {
        private readonly StoreContext _context;

        public NavbarMenuViewComponent(StoreContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            var categories = _context.MenuCategories.OrderBy(c => c.Name);
            return View("MenuCategories", model: categories);
        }
    }
}
