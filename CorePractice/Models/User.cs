using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CorePractice.Models
{
    public class User
    {
        public int UserId { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Mobile { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string Username { get; set; }
        public List<UserGroup> UserGroups { get; set; }

    }
}
