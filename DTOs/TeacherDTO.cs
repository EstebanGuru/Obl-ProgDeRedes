using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DTOs
{
    [Serializable()]
    public class TeacherDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }

        public TeacherDTO(int id, string email, string name, string lastName, string password)
        {
            Email = email;
            Id = id;
            Name = name;
            LastName = lastName;
            Password = password;
        }

    }
}
