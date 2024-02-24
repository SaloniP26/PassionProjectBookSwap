using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PassionProjectBookSwap.Models
{
    public class Genre
    {
        [Key]
        public int GenreID { get; set; }
        public string GenreName { get; set; }

        public ICollection<Book> Books { get; set; }

    }

    public class GenreDto
    {
        public int GenreID { get; set; }

        public string GenreName { get; set; }
    }

    }