using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PassionProjectBookSwap.Models
{
    public class Book
    {
        [Key]
        public int BookID { get; set; }
        public string BookName { get; set; }
        public string BookAuthor { get; set; }
        public DateTime BookPublishDate { get; set; }
        public string BookLocation { get; set; }
        public string BookCaption { get; set; }

        //A book has a user id 
        //A user has many books

        //public int UserID { get; set; }
        [ForeignKey("NewUsers")]
        public int UserID { get; set; }

        public virtual NewUser NewUsers { get; set; }

        //

        [ForeignKey("Genre")]
        public int GenreID { get; set; }
        
        public virtual Genre Genre { get; set; }



    }

    public class BookDto
    {
        public int BookID { get; set; }

        public string BookName { get; set; }

        public string BookAuthor { get; set; }
        public DateTime BookPublishDate { get; set; }
        public string BookLocation { get; set; }
        public string BookCaption { get; set; }

        public int UserID { get; set; }

        public int GenreID { get; set;}

    }
}