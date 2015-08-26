using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Job_Assignment
{
    public class UserInfo
    {
        public string UserId {get; set;}
        public string Name { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }
        public DateTime LastLoginDate { get; set; }
        public String PinStatus { get; set; }
        public Dictionary<string, UserRole> Roles { get; set; }

        public UserInfo()
        {
            UserId = null;
            Name = null;
            LastLoginDate = DateTime.Now;
            PinStatus = null;
        }
    }
}
