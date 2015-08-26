using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Job_Assignment
{
    public class UserRole
    {
        public string UserId { get; set; }
        public string Module { get; set; }
        public bool IsViewOnly { get; set; }
        public bool IsCreate { get; set; }
        public bool IsImport { get; set; }
    }
}
