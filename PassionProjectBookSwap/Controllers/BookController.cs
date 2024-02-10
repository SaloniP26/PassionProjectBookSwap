using Microsoft.AspNet.Identity;
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
    public class BookController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static BookController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44366/api/bookdata/");
        }

        //Objective: A webpage that list the books in our system
        // GET: Book/List
        public ActionResult List()
        {
            //objective: communicate with our book data api to retrieve a list of books
            //curl https://localhost:44324/api/bookdata/listbooks


            string url = "https://localhost:44366/api/bookdata/listbooks";
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            IEnumerable<BookDto> books = response.Content.ReadAsAsync<IEnumerable<BookDto>>().Result;
            //Debug.WriteLine("Number of books received : ");
            //Debug.WriteLine(books.Count());


            return View(books);
        }


        // GET: Book/ListUserBooks/?userId=1
        //Objective: A webpage that list the books in our system with a specific user id
        public ActionResult ListUserBooks(int userId)
        {
            // Retrieve the user ID
           
            ViewBag.UserId = userId;
            Debug.WriteLine(userId);
            //GET {resource}/api/bookdata/listbooks
            //https://localhost:44366/api/bookdata/listbooks/{userId}
            //Use HTTP client to access information

            HttpClient client = new HttpClient();
            //set the url
            string url = $"https://localhost:44366/api/bookdata/listbooks/{userId}";

            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<BookDto> books = response.Content.ReadAsAsync<IEnumerable<BookDto>>().Result;

            return View(books);
        }

        // GET: Book/Details/5
        public ActionResult Details(int id)
        {
            //objective: communicate with our book data api to retrieve one book
            //curl https://localhost:44324/api/bookdata/findbook/{id}

            string url = "findbook/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("The response code is ");
            Debug.WriteLine(response.StatusCode);

            BookDto selectedbook = response.Content.ReadAsAsync<BookDto>().Result;
            Debug.WriteLine("book received : ");
            


            return View(selectedbook);
        }

        public ActionResult Error()
        {

            return View();
        }

        // GET: Book/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Book/Create
        [HttpPost]
        public ActionResult Create(Book book)
        {
            Debug.WriteLine("the json payload is :");
            //Debug.WriteLine(book.BookName);
            //objective: add a new book into our system using the API
            //curl -H "Content-Type:application/json" -d @book.json https://localhost:44324/api/bookdata/addbook
            string url = "addbook";


            string jsonpayload = jss.Serialize(book);

            Debug.WriteLine(jsonpayload);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }

        }

        // GET: Book/Edit/5
        public ActionResult Edit(int id)
        {
            //grab the book information
            ViewBag.UserId = id;
            //objective: communicate with our book data api to retrieve one book
            //curl https://localhost:44324/api/bookdata/findbook/{id}

            string url = "findbook/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            BookDto selectedbook = response.Content.ReadAsAsync<BookDto>().Result;
            //Debug.WriteLine("book received : ");
            //Debug.WriteLine(selectedbook.BookName);

            return View(selectedbook);
        }
        // POST: Book/Update/5
        [HttpPost]
        public ActionResult Update(int id, Book book)
        {
            try
            {
                Debug.WriteLine("The new book info is:");
                Debug.WriteLine(book.BookName);
                Debug.WriteLine(book.BookAuthor);

                //serialize into JSON
                //Send the request to the API

                string url = "UpdateBook/" + id;


                string jsonpayload = jss.Serialize(book);
                Debug.WriteLine(jsonpayload);

                HttpContent content = new StringContent(jsonpayload);
                content.Headers.ContentType.MediaType = "application/json";

                //POST: api/BookData/UpdateBook/{id}
                //Header : Content-Type: application/json
                HttpResponseMessage response = client.PostAsync(url, content).Result;




                return RedirectToAction("Details/" + id);
            }
            catch
            {
                return View();
            }
        }

        // GET: Book/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }


    }
}

