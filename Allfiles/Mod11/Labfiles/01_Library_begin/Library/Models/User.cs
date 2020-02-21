﻿using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Library.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }

        public virtual ICollection<Book> Books { get; set; }
    }
}
