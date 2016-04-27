using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace BPMService.WebUI.Models
{
    public class ContactResponse
    {
        [Required(ErrorMessage = "Please enter your name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter your Mobile Phone number")]
        //[RegularExpression("?+[0-9]+(?:-)", ErrorMessage = "Please enter a valid mobile phone number only with digit signs")]
        public string MobilePhone { get; set; }
        
        [Required(ErrorMessage = "Please enter your job title")]
        public string JobTitle { get; set; }

        [Required(ErrorMessage = "Please enter your birth date")]
        public string BirthDate { get; set; }
    }
}