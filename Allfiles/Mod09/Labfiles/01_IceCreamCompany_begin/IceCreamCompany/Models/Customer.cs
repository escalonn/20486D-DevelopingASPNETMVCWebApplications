using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IceCreamCompany.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "Please enter your first name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Please enter your last name")]
        public string LastName { get; set; }

        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Please enter your email address")]
        public string Email { get; set; }

        [Display(Name = "Phone"), DataType(DataType.PhoneNumber)]
        public long PhoneNumber { get; set; }

        [Required(ErrorMessage = "Please enter your address")]
        public string Address { get; set; }

        public List<IceCreamFlavorsCustomers> IceCreamFlavors { get; set; }
    }
}
