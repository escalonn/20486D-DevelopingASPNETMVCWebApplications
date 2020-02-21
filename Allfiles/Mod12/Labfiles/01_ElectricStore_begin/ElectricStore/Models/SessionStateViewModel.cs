using System.Collections.Generic;

namespace ElectricStore.Models
{
    public class SessionStateViewModel
    {
        public string CustomerName { get; set; }
        public List<Product> SelectedProducts { get; set; }
    }
}
