using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace PassionProjectBookSwap.Models
{
    public class NewUser
    {
        [Key]
        public int UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailID { get; set; }
        public int Phone { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }


        //a user has many books
        public ICollection<Book> Books {get; set;}

    }
    public class NewUserDto
    {
        public int UserID { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailID { get; set; }
        public int Phone { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

    }
}