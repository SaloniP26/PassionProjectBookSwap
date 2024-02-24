using PassionProjectBookSwap.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace PassionProjectBookSwap.Controllers
{
    public class GenreDataController : ApiController
    {
       
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/GenreData/ListGenres
        [HttpGet]
        [Route("api/GenreData/ListGenres")]
        public IHttpActionResult ListGenres()
        {
            List<Genre> genres = db.Genres.ToList();

            List<GenreDto> genreDtos = genres.Select(g => new GenreDto
            {
                GenreID = g.GenreID,
                GenreName = g.GenreName
            }).ToList();

            return Ok(genreDtos);
        }

        // GET: api/GenreData/FindGenre/5
        [ResponseType(typeof(Genre))]
        [HttpGet]
        public IHttpActionResult FindGenre(int id)
        {
            Genre genre = db.Genres.Find(id);
            if (genre == null)
            {
                return NotFound();
            }

            GenreDto genreDto = new GenreDto()
            {
                GenreID = genre.GenreID,
                GenreName = genre.GenreName
            };

            return Ok(genreDto);
        }

        //FindBook

        // GET: api/BookData/FindBook/5
        [ResponseType(typeof(Book))]
        [HttpGet]
        public IHttpActionResult BookForGenre(int id)
        {
            List<Book> Book = db.Books.Where( g => g.GenreID == id).ToList();

            if (Book == null)
            {
                return NotFound();
            }

            List<BookDto> bookDtos = Book.Select(b => new BookDto
            {
                BookID = b.BookID,
                BookName = b.BookName,
                BookAuthor = b.BookAuthor,
                BookPublishDate = b.BookPublishDate,
                BookLocation = b.BookLocation,
                BookCaption = b.BookCaption,
                UserID = b.UserID
            }).ToList();

            return Ok(bookDtos);
        }


        // POST: api/GenreData/AddGenre
        [ResponseType(typeof(Genre))]
        [HttpPost]
        public IHttpActionResult AddGenre(Genre genre)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Genres.Add(genre);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = genre.GenreID }, genre);
        }

        // POST: api/GenreData/UpdateGenre/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateGenre(int id, Genre genre)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != genre.GenreID)
            {
                return BadRequest();
            }

            db.Entry(genre).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GenreExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/GenreData/DeleteGenre/5
        [ResponseType(typeof(Genre))]
        [HttpPost]
        public IHttpActionResult DeleteGenre(int id)
        {
            Genre genre = db.Genres.Find(id);
            if (genre == null)
            {
                return NotFound();
            }

            db.Genres.Remove(genre);
            db.SaveChanges();

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool GenreExists(int id)
        {
            return db.Genres.Count(e => e.GenreID == id) > 0;
        }
            }
        }