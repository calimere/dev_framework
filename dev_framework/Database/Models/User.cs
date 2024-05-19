using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dev_framework.Models
{
    public abstract class AUser : IdentityUser
    {
        public string usr_first_name { get; set; }
        public string usr_last_name { get; set; }
        public bool is_deleted { get; set; }
        public DateTime created { get; set; }

        [NotMapped]
        public string FullName
        {
            get
            {
                return usr_first_name + " " + usr_last_name;
            }
        }
    }
}
