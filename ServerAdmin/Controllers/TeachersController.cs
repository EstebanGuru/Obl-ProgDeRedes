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
using LogsLibrary;
using System.Configuration;

namespace ServerAdmin.Controllers
{
    public class TeachersController : ApiController
    {
        private ServerAdminEntities db = new ServerAdminEntities();
        private LogsLogic Logs = new LogsLogic(ConfigurationManager.AppSettings.Get("LocalPrivateQueue")); 

        // GET: api/Teachers
        public IQueryable<Teacher> GetTeachers()
        {
            return db.Teachers;
        }

        // GET: api/Teachers/5
        [ResponseType(typeof(Teacher))]
        public IHttpActionResult GetTeacher(Guid id)
        {
            Teacher teacher = db.Teachers.Find(id);
            if (teacher == null)
            {
                return NotFound();
            }

            return Ok(teacher);
        }

        // PUT: api/Teachers/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTeacher(Guid id, Teacher teacher)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != teacher.Id)
            {
                return BadRequest();
            }

            db.Entry(teacher).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TeacherExists(id))
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

        // POST: api/Teachers
        [ResponseType(typeof(Teacher))]
        public IHttpActionResult PostTeacher(TeacherDTO teacherDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Teacher teacher = new Teacher()
            {
                Name = teacherDTO.Name,
                LastName = teacherDTO.LastName,
                Email = teacherDTO.Email,
                Password = teacherDTO.Password,
                Id = Guid.NewGuid(),
            };

            db.Teachers.Add(teacher);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException e)
            {
                if (TeacherExists(teacher.Id))
                {
                    return Conflict();
                }
                else
                {
                    BadRequest("Some error in data base " + e.Message);
                }
            }
            Logs.SendTimestamp("CreateStudent", "admin", "Teacher added: " + teacherDTO.Name);
            return Ok("Teacher created succesfully");
        }

        // DELETE: api/Teachers/5
        [ResponseType(typeof(Teacher))]
        public IHttpActionResult DeleteTeacher(Guid id)
        {
            Teacher teacher = db.Teachers.Find(id);
            if (teacher == null)
            {
                return NotFound();
            }

            db.Teachers.Remove(teacher);
            db.SaveChanges();

            return Ok(teacher);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TeacherExists(Guid id)
        {
            return db.Teachers.Count(e => e.Id == id) > 0;
        }
    }
}