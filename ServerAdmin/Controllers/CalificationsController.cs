using IRemotingCourseLogic;
using ServerAdmin.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace ServerAdmin.Controllers
{
    public class CalificationsController : ApiController
    {
        private string RemoteAddress = ConfigurationManager.AppSettings["RemotingAddress"];
        [Route("api/Calification")]
        [ResponseType(typeof(string))]
        public IHttpActionResult PostCalificaion(StudentCourseDTO calification)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var remoteServer = (ICourseLogic)Activator.GetObject(
                typeof(ICourseLogic),
                RemoteAddress);
            string response = remoteServer.AddCalificationRemoting(calification.CourseName, calification.StudentId, calification.Calification);
            if (response.Equals("OK"))
            {
                return Ok(response);
            } else
            {
                return BadRequest(response);
            }            
        }

        [Route("api/Califications")]
        [ResponseType(typeof(List<StudentCourse>))]
        public IHttpActionResult GetTopCalifications()
        {
            try
            {
                var remoteServer = (ICourseLogic)Activator.GetObject(
                typeof(ICourseLogic),
                RemoteAddress);
                List<StudentCourse> response = remoteServer.GetTopCalifications();
                return Ok(response);

            }
            catch (Exception e)
            {
                return BadRequest("Error trying to get califications");
            }
            
        }
    }
}
