using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FinalProject.Models
{
    public class EVisa
    {
        [Key]
        public int ApplicationID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string VisaService { get; set; }
        public string Gender { get; set; }
        public string Nationality { get; set; }
        public string PassportNumber { get; set; }
        public byte[] Photo { get; set; }
        public string ETANumber { get; set; }
        public int NoOfEntries { get; set; }
        public DateTime DateOfIssueOfETA { get; set; }
        public DateTime DateOfExpiryOfETA { get; set; }
        public string ApplicationStatus { get; set; }
    }
}