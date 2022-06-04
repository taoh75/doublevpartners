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
    [EnableCors(origins:"*", headers:"*", methods: "*")]
    public class personController : ApiController
    {
        private DoubleVPartnersEntities db = new DoubleVPartnersEntities();

        // GET: api/person
        public IQueryable<person> Getperson()
        {
            return db.person;
        }

        // GET: api/person/5
        [ResponseType(typeof(person))]
        public IHttpActionResult Getperson(int id)
        {
            person person = db.person.Find(id);
            if (person == null)
            {
                return NotFound();
            }

            return Ok(person);
        }

        // PUT: api/person/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putperson(int id, person person)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != person.id)
            {
                return BadRequest();
            }

            db.Entry(person).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!personExists(id))
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

        // POST: api/person
        [ResponseType(typeof(person))]
        public IHttpActionResult Postperson(person person)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.person.Add(person);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = person.id }, person);
        }

        // DELETE: api/person/5
        [ResponseType(typeof(person))]
        public IHttpActionResult Deleteperson(int id)
        {
            person person = db.person.Find(id);
            if (person == null)
            {
                return NotFound();
            }

            db.person.Remove(person);
            db.SaveChanges();

            return Ok(person);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool personExists(int id)
        {
            return db.person.Count(e => e.id == id) > 0;
        }
    }
}