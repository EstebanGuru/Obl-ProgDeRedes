using IRemotingCourseLogic;
using ServerAdmin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace ServerAdmin.Controllers
{
    public class CalificationsController : ApiController
    {
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
                "tcp://192.168.1.45:7000/courseLogicService");
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
                "tcp://192.168.1.45:7000/courseLogicService");
                List<StudentCourse> response = remoteServer.GetTopCalifications();
                return Ok(response);

            }
            catch (Exception)
            {
                return BadRequest("Error trying to get califications");
            }
            
        }
    }
}
