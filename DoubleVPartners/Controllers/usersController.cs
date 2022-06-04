using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using DoubleVPartners.Models;

namespace DoubleVPartners.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class usersController : ApiController
    {
        private DoubleVPartnersEntities db = new DoubleVPartnersEntities();

        // GET: api/users
        public IQueryable<user> Getuser()
        {
            return db.user;
        }

        // GET: api/users/5
        [ResponseType(typeof(user))]
        public IHttpActionResult Getuser(int id)
        {
            user user = db.user.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        // PUT: api/users/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putuser(int id, user user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user.id)
            {
                return BadRequest();
            }

            db.Entry(user).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!userExists(id))
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

        // POST: api/users
        [ResponseType(typeof(user))]
        public IHttpActionResult Postuser(user user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
                        
            db.user.Add(user);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = user.id }, user);
        }

        //Esta función le da acceso al Usuario 
        [ResponseType(typeof(user))]
        [Route("api/login")]
        [HttpPost]
        // POST: api/login
        public IHttpActionResult Loginuser(user user)
        {
            user userLog = db.user.FirstOrDefault(x => x.loguser == user.loguser);
            if (userLog == null)
            {
                return NotFound();
            }

            if (userLog.pass != user.pass) { return NotFound(); }

            return Ok(userLog);
        }        

        // DELETE: api/users/5
        [ResponseType(typeof(user))]
        public IHttpActionResult Deleteuser(int id)
        {
            user user = db.user.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            db.user.Remove(user);
            db.SaveChanges();

            return Ok(user);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool userExists(int id)
        {
            return db.user.Count(e => e.id == id) > 0;
        }
    }
}