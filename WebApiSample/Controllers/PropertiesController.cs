//using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using WebApiSample.Models;

namespace WebApiSample.Controllers
{
    public class PropertiesController : ApiController
    {
        private RealEstateDBEntities db = new RealEstateDBEntities();

        // GET: api/Properties
        public IQueryable<property> Getproperties()
        {
            return db.properties;
        }

        // GET: api/Properties/5
        [ResponseType(typeof(property))]
        public async Task<IHttpActionResult> Getproperty(int id)
        {
            property property = await db.properties.FindAsync(id);
            if (property == null)
            {
                return NotFound();
            }

            return Ok(property);
        }

        // PUT: api/Properties/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> Putproperty(int id, property property)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != property.id)
            {
                return BadRequest();
            }

            db.Entry(property).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!propertyExists(id))
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

        // POST: api/Properties
        [ResponseType(typeof(property))]
        public async Task<IHttpActionResult> Postproperty(property property)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.properties.Add(property);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = property.id }, property);
        }

        // DELETE: api/Properties/5
        [ResponseType(typeof(property))]
        public async Task<IHttpActionResult> Deleteproperty(int id)
        {
            property property = await db.properties.FindAsync(id);
            if (property == null)
            {
                return NotFound();
            }

            db.properties.Remove(property);
            await db.SaveChangesAsync();

            return Ok(property);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool propertyExists(int id)
        {
            return db.properties.Count(e => e.id == id) > 0;
        }
    }
}