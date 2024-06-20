using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class ApplicationUser:IdentityUser
    {
        [PersonalData]
        public string Name { get; set; }
        [PersonalData]
        public string? StreetName { get; set; }
        [PersonalData]
        public string? City { get; set; }
        [PersonalData]
        public string? State { get; set; }
        [PersonalData]
        public string? PostalCode { get; set;}
        
        
    }
}
