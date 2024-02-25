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
    public class NewUserController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        private ApplicationDbContext db = new ApplicationDbContext();

        static NewUserController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44366/api/newuserdata/");
        }

        //Objective: A webpage that list the books in our system
        // GET: Book/List
        public ActionResult List()
        {
            //objective: communicate with our book data api to retrieve a list of books
            //curl https://localhost:44324/api/newuserdata/listnewusers


            string url = "https://localhost:44366/api/newuserdata/listnewusers";
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            IEnumerable<NewUserDto> newusers = response.Content.ReadAsAsync<IEnumerable<NewUserDto>>().Result;
            //Debug.WriteLine("Number of newusers received : ");
            //Debug.WriteLine(books.Count());


            return View(newusers);
        }


        // GET: NewUser/ListUserNewUsers/?userId=1
        //Objective: A webpage that list the newusers in our system with a specific user id
        public ActionResult ListUserNewUsers(int userId)
        {
            // Retrieve the user ID

            ViewBag.UserId = userId;
            Debug.WriteLine(userId);
            //GET {resource}/api/newuserdata/listnewusers
            //https://localhost:44366/api/newuserdata/listnewusers/{userId}
            //Use HTTP client to access information

            HttpClient client = new HttpClient();
            //set the url
            string url = $"https://localhost:44366/api/newuserdata/listnewusers/{userId}";

            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<NewUserDto> newusers = response.Content.ReadAsAsync<IEnumerable<NewUserDto>>().Result;

            return View(newusers);
        }

        // GET: NewUser/Details/5
        public ActionResult Details(int id)
        {
            //objective: communicate with our newuser data api to retrieve one newuser
            //curl https://localhost:44324/api/newuserdata/findnewuser/{id}

            string url = "findnewuser/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            // Debug.WriteLine("The response code is ");
            // Debug.WriteLine(response.StatusCode);

            NewUserDto selectednewuser = response.Content.ReadAsAsync<NewUserDto>().Result;
            // Debug.WriteLine("book received : ");


            Debug.WriteLine("----" + selectednewuser.UserID);

            return View(selectednewuser);
        }

        public ActionResult Error()
        {

            return View();
        }

        // GET: NewUser/New
        public ActionResult New()
        {
            // Retrieve list of genres from your data source
            //List<Genre> genres = db.Genres.ToList();

            // Pass the list of genres to the view
            //ViewBag.Genres = genres;
            //ViewBag.UserID = UserID;

            return View();
        }

        // POST: NewUser/Create
        [HttpPost]
        public ActionResult Create(NewUser newuser)
        {
            // Debug.WriteLine("the json payload is :");
            //Debug.WriteLine(newuser.NewUserName);
            //objective: add a new newuser into our system using the API
            //curl -H "Content-Type:application/json" -d @newuser.json https://localhost:44324/api/newuserdata/addnewuser
            string url = "addnewuser";


            string jsonpayload = jss.Serialize(newuser);

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
                Debug.WriteLine("---");

                return RedirectToAction("Error");
            }

        }

        // GET: NewUser/Edit/5
        public ActionResult Edit(int id)
        {
            //grab the newuser information
            ViewBag.UserId = id;
            //objective: communicate with our newuser data api to retrieve one book
            //curl https://localhost:44324/api/newuserdata/findnewuser/{id}

            string url = "findnewuser/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            NewUserDto selectednewuser = response.Content.ReadAsAsync<NewUserDto>().Result;
            //Debug.WriteLine("newuser received : ");
            //Debug.WriteLine(selectedbook.BookName);

            return View(selectednewuser);
        }
        // POST: NewUser/Update/5
        [HttpPost]
        public ActionResult Update(int id, NewUser newuser)
        {

            //Debug.WriteLine("Genre ID received: " + genreId);
            Debug.WriteLine(newuser.FirstName);
            Debug.WriteLine("The new newuser info is-----------------------------:");

            Debug.WriteLine(newuser.LastName);
            //Debug.WriteLine(genreId);

            //book.GenreID = genreId;

            //serialize into JSON
            //Send the request to the API

            string url = "updatenewuser/" + id;

            string jsonpayload = jss.Serialize(newuser);
            Debug.WriteLine(jsonpayload);




            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            //POST: api/NewUserData/UpdateNewUser/{id}
            //Header : Content-Type: application/json
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                Debug.WriteLine("okay-:");

                return RedirectToAction("Details/" + id);
            }
            else
            {
                Debug.WriteLine("not okay-:");

                return RedirectToAction("Details/" + id);
            }


        }

        // GET: NewUser/Delete/5
        public ActionResult Delete(int id, string FirstName)
        {
            ViewBag.id = id;
            ViewBag.FirstName = FirstName;

            return View();
        }

        // GET: NewUser/Delete/5
        public ActionResult ConfirmDelete(int id)
        {
            Debug.WriteLine("---- " + id);
            // call api


            // Call the API to confirm the delete action
            string url = "deletenewuser/" + id;


            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                Debug.WriteLine("NewUser deleted successfully.");
            }
            else
            {
                Debug.WriteLine("Failed to delete the NewUser.");
            }

            return RedirectToAction("List");
        }

        public ActionResult BooksForUser(int id)
        {
            // Retrieve newusers for the specified user ID
            string url = "https://localhost:44366/api/bookdata/booksforuser/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                IEnumerable<NewUserDto> newusers = response.Content.ReadAsAsync<IEnumerable<NewUserDto>>().Result;
                ViewBag.id = id;
                return View(newusers);
            }
            else
            {
                // Handle error response
                Debug.WriteLine("Failed to retrieve books for genre with ID: " + id);
                return RedirectToAction("Error");
            }
        }

    }


}