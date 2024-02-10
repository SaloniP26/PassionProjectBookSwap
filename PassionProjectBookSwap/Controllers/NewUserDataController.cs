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

        //This Controller Will access the NewUsers table of our passion project database. Non-Deterministic.
        /// <summary>
        /// Returns a list of NewUsers in the system
        /// </summary>
        /// <returns>
        /// A list of NewUsers Objects with fields mapped to the database column values (first name, last name, email id, phone, username & password).
        /// </returns>
        /// <example>GET api/NewUserData/ListNewUsers -> {NewUsers Object, NewUsers Object, NewUsers Object...}</example>

        //ListNewUsers

        [HttpGet]
        [Route("api/NewUserData/ListNewUsers")]
        public List<NewUserDto> ListNewUsers()
        {
            //sending a query to the database
            //select * from neusers...
            List<NewUser> NewUsers  = db.NewUsers.ToList();

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

            //push the results to the list of newuserss to return

            return NewUserDtos;
        }


        /// <summary>
        /// Finds an NewUser from the Database through an id. Non-Deterministic.
        /// </summary>
        /// <param name="id">The User ID</param>
        /// <returns>Book object containing information about the NewUser with a matching ID. Empty NewUser Object if the ID does not match any NewUser in the system.</returns>
        /// <example>api/NewUserData/FindNewUser/5 -> {NewUser Object}</example>
        /// <example>api/NewUserData/FindNewUser/5 -> {NewUser Object}</example>


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
        /// Adds an NewUser to the Database. Non-Deterministic.
        /// </summary>
        /// <param name="NewUser">An object with fields that map to the columns of the NewUser's table. </param>
        /// <example>
        /// POST api/NewUserData/AddNewUser
        /// FORM DATA / POST DATA / REQUEST BODY 
        /// {
        ///	"FirstName": "Saloni",
        ///"LastName": "Pawar",
        ///"EmailID": "salonip@gmail.com",
        ///"Phone": "8765537",
        ///"UserName": "SaloniP",
        ///"Password": "Test@123",
        ///"UserID": 1
        /// }
        /// </example>

        //AddNewUser

        // POST: api/NewUserData/AddNewUser
        [ResponseType(typeof(NewUser))]
        [HttpPost]
        public IHttpActionResult AddNewUser(NewUser newuser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.NewUsers.Add(newuser);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = newuser.UserID }, newuser);
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
        ///"EmailID": "salonip@gmail.com",
        ///"Phone": "8765537",
        ///"UserName": "SaloniP",
        ///"Password": "Test@123",
        ///"UserID": 1
        /// }
        /// </example>

        //UpdateNewUser

        // POST: api/NewUserData/UpdateNewUser/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateNewUser(int id, NewUser newuser)
        {
            Debug.WriteLine("I have reached the update newuser method!");
            if (!ModelState.IsValid)
            {
                Debug.WriteLine("Model State is invalid");
                return BadRequest(ModelState);
            }

            if (id != newuser.UserID)
            {
                Debug.WriteLine("ID mismatch");
                Debug.WriteLine("GET parameter" + id);
                Debug.WriteLine("POST parameter" + newuser.UserID);
                Debug.WriteLine("POST parameter" + newuser.FirstName);
                return BadRequest();
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
                    Debug.WriteLine("NewUser not found");
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
        /// <example>POST /api/NewUserData/DeleteNewUser/5</example>

        //DeleteNewUser

        // POST: api/NewUserData/DeleteNewUser/5
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

        // List books based on userId

       // db.Books.Where( u => u.UserID == id)

        //Related Methods included:

        //ListNewUsersForGenre

        //AddNewUserToGenre

        //RemoveNewUserFromGenre

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
