using PassionProjectBookSwap.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace PassionProjectBookSwap.Controllers
{
    public class GenreController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static GenreController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44366/api/genredata/");
        }

        // GET: Genre/List
        public ActionResult List()
        {
            string url = "https://localhost:44366/api/genredata/listgenres";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<GenreDto> genres = response.Content.ReadAsAsync<IEnumerable<GenreDto>>().Result;

            return View(genres);
        }

        // GET: Genre/BooksForGenre/5
        public ActionResult BooksForGenre(int id)
        {
            // Retrieve the genre from the database
            var genre = db.Genres.Find(id);
            if (genre == null)
            {
                return HttpNotFound();
            }

            // Retrieve books associated with the genre
            var booksForGenre = db.Books.Where(b => b.GenreID == id).ToList();

            // Map Book entities to BookDto objects
            var bookDtos = booksForGenre.Select(b => new BookDto
            {
                BookID = b.BookID,
                BookName = b.BookName,
                BookAuthor = b.BookAuthor,
                BookPublishDate = b.BookPublishDate,
                BookLocation = b.BookLocation,
                BookCaption = b.BookCaption,
                UserID = b.UserID
            });

            // Pass the genre name and associated book DTOs to the view
            ViewBag.GenreName = genre.GenreName;
            return View(booksForGenre);
        }

    }
}
