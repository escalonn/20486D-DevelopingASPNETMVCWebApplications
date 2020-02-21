using System.Collections.Generic;

namespace Library.Models
{
    public class User
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }

        public virtual ICollection<Book> Books { get; set; }
    }
}
