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
    public class NewUserDataController : ApiController
    {
        //utilizing the database connection
        private ApplicationDbContext db = new ApplicationDbContext();

        //This Controller Will access the users table of our passion project database. Non-Deterministic.
        /// <summary>
        /// Returns a list of users in the system
        /// </summary>
        /// <returns>
        /// A list of users Objects with fields mapped to the database column values (user first name, user last name, user email, user phone, user's username and user's password).
        /// </returns>
        /// <example>GET api/BookData/ListNewUsers -> {User Object, User Object, User Object...}</example>

        //ListUser

        [HttpGet]
        [Route("api/NewUserData/ListNewUsers")]
        public List<NewUserDto> ListNewUsers()
        {
            //sending a query to the database
            //select * from users...
            List<NewUser> NewUsers = db.NewUsers.ToList();

            List<NewUserDto> NewUserDtos = new List<NewUserDto>();

            //read through the results..

            NewUsers.ForEach(b => NewUserDtos.Add(new NewUserDto()
            {
                UserID = b.UserID,
                FirstName = b.FirstName,
                LastName = b.LastName,
                EmailID = b.EmailID,
                Phone = b.Phone,
                UserName = b.UserName,
                Password = b.Password
            }
            ));


            //push the results to the list of users to return

            return NewUserDtos;
        }

        // GET: api/newuserdata/listnewusers/{userId}
        [HttpGet]
        [Route("api/newuserdata/listnewusers/{userId}")]
        public IHttpActionResult ListNewUserByUser(int userId)
        {
            // Filter newusers based on the provided user ID
            List<NewUser> newusers = db.NewUsers.Where(b => b.UserID == userId).ToList();

            List<NewUserDto> newuserDtos = newusers.Select(b => new NewUserDto
            {
                UserID = b.UserID,
                FirstName = b.FirstName,
                LastName = b.LastName,
                EmailID = b.EmailID,
                Phone = b.Phone,
                UserName = b.UserName,
                Password = b.Password
            }).ToList();

            return Ok(newuserDtos);
        }


        /// <summary>
        /// Finds an User from the Database through an id. Non-Deterministic.
        /// </summary>
        /// <param name="id">The User ID</param>
        /// <returns>User object containing information about the User with a matching ID. Empty User Object if the ID does not match any User in the system.</returns>
        /// <example>api/NewUserData/FindNewUser/6 -> {NewUser Object}</example>
        /// <example>api/NewUserData/FindBook/10 -> {NewUser Object}</example>

        //FindNewUser

        // GET: api/NewUserData/FindNewUser/5
        [ResponseType(typeof(NewUser))]
        [HttpGet]
        public IHttpActionResult FindNewUser(int id)
        {
            NewUser NewUser = db.NewUsers.Find(id);
            if (NewUser == null)
            {
                return NotFound();
            }

            NewUserDto NewUserDto = new NewUserDto()
            {
                UserID = NewUser.UserID,
                FirstName = NewUser.FirstName,
                LastName = NewUser.LastName,
                EmailID = NewUser.EmailID,
                Phone = NewUser.Phone,
                UserName = NewUser.UserName,
                Password = NewUser.Password

            };

            return Ok(NewUserDto);
        }

        /// <summary>
        /// Adds an Newuser to the Database. Non-Deterministic.
        /// </summary>
        /// <param name="newuser">An object with fields that map to the columns of the NewUser's table. </param>
        /// <example>
        /// POST api/NewUserData/AddNewUser
        /// FORM DATA / POST DATA / REQUEST BODY 
        /// {
        ///	"FirstName": "Saloni",
        ///"LastName": "Pawar",
        ///"EmailId": "saloni@gmail.com",
        ///"Phone": "4125614236",
        ///"Username": "test",
        ///"Password": "test@123",
        ///"UserID": 1
        /// }
        /// </example>

        //AddNewUser

        // POST: api/NewUserData/AddNewUser


        [HttpPost]
        public IHttpActionResult AddNewUser(NewUser newuser)
        {
            Debug.WriteLine("in add book api");

            Debug.WriteLine(newuser.FirstName + "in api");

            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.NewUsers.Add(newuser);
            db.SaveChanges();

            return Ok();
        }


        /// <summary>
        /// Updates an NewUser on the Database. Non-Deterministic.
        /// </summary>
        /// <param name="newuser">An object with fields that map to the columns of the NewUser's table.</param>
        /// <example>
        /// POST api/NewUserData/UpdateNewUser/5 
        /// FORM DATA / POST DATA / REQUEST BODY 
        /// {
        ///	"FirstName": "Saloni",
        ///"LastName": "Pawar",
        ///"EmailId": "saloni@gmail.com",
        ///"Phone": "4125614236",
        ///"Username": "test",
        ///"Password": "test@123",
        ///"UserID": 1
        /// }
        /// </example>


        //UpdateNewUser

        // POST: api/NewUserData/UpdateNewUser/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateNewUser(int id, NewUser newuser)
        {

            Debug.WriteLine("--7777777777777777777777777777777-" + newuser.FirstName);
            Debug.WriteLine("I have reached the update newuser method!");
            if (!ModelState.IsValid)
            {
                Debug.WriteLine("Model State is invalid");
                return BadRequest(ModelState);
            }



            db.Entry(newuser).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NewUserExists(id))
                {
                    Debug.WriteLine("User not found");
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
        /// Deletes an NewUser from the connected Database if the ID of that NewUser exists. Does NOT maintain relational integrity. Non-Deterministic.
        /// </summary>
        /// <param name="id">The ID of the NewUser.</param>
        /// <example>POST /api/NewUserData/DeleteNewUser/3</example>

        //DeleteNewUser

        // POST: api/NewUserDataData/DeleteNewUserData/5
        [ResponseType(typeof(NewUser))]
        [HttpPost]
        public IHttpActionResult DeleteNewUser(int id)
        {
            NewUser NewUser = db.NewUsers.Find(id);
            if (NewUser == null)
            {
                return NotFound();
            }

            db.NewUsers.Remove(NewUser);
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
        private bool NewUserExists(int id)
        {
            return db.NewUsers.Count(e => e.UserID == id) > 0;
        }
    }
}
