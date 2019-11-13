using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using ServerAdmin;
using ServerAdmin.Models;

namespace ServerAdmin.Controllers
{
    public class SessionsController : ApiController
    {
        private ServerAdminEntities db = new ServerAdminEntities();

        [Route("api/login")]
        [ResponseType(typeof(Session))]
        public IHttpActionResult PostSession(Credentials credentials)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IEnumerable<Teacher> teachers = db.Teachers;
            Teacher teacher = teachers.Where(t => t.Email == credentials.Email && t.Password == credentials.Password).FirstOrDefault();
            if (teacher == null) return BadRequest("Invalid credentials");
            IEnumerable<Session> sessions = db.Sessions;
            Session session = sessions.Where(s => s.Teacher == teacher.Id).FirstOrDefault();
            if (session == null)
            {
                session = new Session() { Teacher = teacher.Id, Id = Guid.NewGuid() };
            }
            db.Sessions.Add(session);
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException e)
            {
                if (SessionExists(session.Id))
                {
                    return Conflict();
                }
                else
                {
                    BadRequest("Some error in data base " + e.Message);
                }
            }

            return Ok(session);
        }

        // DELETE: api/Sessions/5
        [ResponseType(typeof(Session))]
        public IHttpActionResult DeleteSession(Guid id)
        {
            Session session = db.Sessions.Find(id);
            if (session == null)
            {
                return NotFound();
            }

            db.Sessions.Remove(session);
            db.SaveChanges();

            return Ok(session);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SessionExists(Guid id)
        {
            return db.Sessions.Count(e => e.Id == id) > 0;
        }
    }
}