using PassionProjectBookSwap.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;


namespace PassionProjectBookSwap.Controllers
{
    public class BookDataController : ApiController
    {
        //utilizing the database connection
        private ApplicationDbContext db = new ApplicationDbContext();

        //This Controller Will access the books table of our passion project database. Non-Deterministic.
        /// <summary>
        /// Returns a list of books in the system
        /// </summary>
        /// <returns>
        /// A list of Books Objects with fields mapped to the database column values (book name, book author, book genre, book publish date, book location & book caption).
        /// </returns>
        /// <example>GET api/BookData/ListBooks -> {Book Object, Book Object, Book Object...}</example>

        //ListBook

        [HttpGet]
        [Route("api/BookData/ListBooks")]
        public List<BookDto> ListBooks()
        {
            //sending a query to the database
            //select * from books...
            List<Book> Books = db.Books.ToList();

            List<BookDto> BookDtos = new List<BookDto>();

            //read through the results..

            Books.ForEach(b => BookDtos.Add(new BookDto()
            {
                BookID = b.BookID,
                BookName = b.BookName,
                BookAuthor = b.BookAuthor,
                BookPublishDate = b.BookPublishDate,
                BookLocation = b.BookLocation,
                BookCaption = b.BookCaption
            }
            ));


            //push the results to the list of books to return

            return BookDtos;
        }

        // GET: api/bookdata/listbooks/{userId}
        [HttpGet]
        [Route("api/bookdata/listbooks/{userId}")]
        public IHttpActionResult ListBooksByUser(int userId)
        {
            // Filter books based on the provided user ID
            List<Book> books = db.Books.Where(b => b.UserID == userId).ToList();

            List<BookDto> bookDtos = books.Select(b => new BookDto
            {
                BookID = b.BookID,
                BookName = b.BookName,
                BookAuthor = b.BookAuthor,
                BookPublishDate = b.BookPublishDate,
                BookLocation = b.BookLocation,
                BookCaption = b.BookCaption
            }).ToList();

            return Ok(bookDtos);
        }


        /// <summary>
        /// Finds an Book from the Database through an id. Non-Deterministic.
        /// </summary>
        /// <param name="id">The Book ID</param>
        /// <returns>Book object containing information about the Book with a matching ID. Empty Book Object if the ID does not match any Book in the system.</returns>
        /// <example>api/BookData/FindBook/6 -> {Book Object}</example>
        /// <example>api/BookData/FindBook/10 -> {Book Object}</example>

        //FindBook

        // GET: api/BookData/FindBook/5
        [ResponseType(typeof(Book))]
        [HttpGet]
        public IHttpActionResult FindBook(int id)
        {
            Book Book = db.Books.Find(id);
            if (Book == null)
            {
                return NotFound();
            }

            BookDto BookDto = new BookDto()
            {
                BookID = Book.BookID,
                BookName = Book.BookName,
                BookAuthor = Book.BookAuthor,
                BookPublishDate = Book.BookPublishDate,
                BookLocation = Book.BookLocation,
                BookCaption = Book.BookCaption,
                UserID = Book.UserID
                
            };
            
            return Ok(BookDto);
        }

        /// <summary>
        /// Adds an Book to the Database. Non-Deterministic.
        /// </summary>
        /// <param name="book">An object with fields that map to the columns of the Book's table. </param>
        /// <example>
        /// POST api/BookData/AddBook
        /// FORM DATA / POST DATA / REQUEST BODY 
        /// {
        ///	"BookName": "The Little Prince",
        ///"BookAuthor": "Antoine de Saint-Exupéry",
        ///"BookGenre": "Fantasy",
        ///"BookPublishDate": "01/07/1943",
        ///"BookLocation": "Etobicoke",
        ///"BookCaption": "Test",
        ///"UserID": 1
        /// }
        /// </example>

        //AddBook

        // POST: api/BookData/AddBook
        [ResponseType(typeof(Book))]
        [HttpPost]
        public IHttpActionResult AddBook(Book book)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Books.Add(book);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = book.BookID }, book);
        }


        /// <summary>
        /// Updates an Book on the Database. Non-Deterministic.
        /// </summary>
        /// <param name="book">An object with fields that map to the columns of the Book's table.</param>
        /// <example>
        /// POST api/BookData/UpdateBook/5 
        /// FORM DATA / POST DATA / REQUEST BODY 
        /// {
        ///	"BookName": "The Little Prince",
        ///"BookAuthor": "Antoine de Saint-Exupéry",
        ///"BookGenre": "Fantasy",
        ///"BookPublishDate": "01/07/1943",
        ///"BookLocation": "Etobicoke",
        ///"BookCaption": "Test",
        ///"UserID": 1
        /// }
        /// </example>


        //UpdateBook

        // POST: api/BookData/UpdateBook/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateBook(int id, Book book)
        {
            Debug.WriteLine("---" + book.BookName);
            Debug.WriteLine("I have reached the update book method!");
            if (!ModelState.IsValid)
            {
                Debug.WriteLine("Model State is invalid");
                return BadRequest(ModelState);
            }

            if (id != book.BookID)
            {
                Debug.WriteLine("ID mismatch");
                Debug.WriteLine("GET parameter" + id);
                Debug.WriteLine("POST parameter" + book.BookID);
                Debug.WriteLine("POST parameter" + book.BookName);
                return BadRequest();
            }

            db.Entry(book).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(id))
                {
                    Debug.WriteLine("Book not found");
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            Debug.WriteLine("None of the conditions triggered");
            return StatusCode(HttpStatusCode.NoContent);
        }


        /// <summary>
        /// Deletes an Book from the connected Database if the ID of that Book exists. Does NOT maintain relational integrity. Non-Deterministic.
        /// </summary>
        /// <param name="id">The ID of the Book.</param>
        /// <example>POST /api/BookData/DeleteBook/3</example>

        //DeleteBook

        // POST: api/BookData/DeleteBook/5
        [ResponseType(typeof(Book))]
        [HttpPost]
        public IHttpActionResult DeleteBook(int id)
        {
            Book Book = db.Books.Find(id);
            if (Book == null)
            {
                return NotFound();
            }

            db.Books.Remove(Book);
            db.SaveChanges();

            return Ok();
        }

        //Related Methods included:

        //ListBooksForGenre

        //AddBookToGenre

        //RemoveBookFromGenre

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        private bool BookExists(int id)
        {
            return db.Books.Count(e => e.BookID == id) > 0;
        }
    }
}
