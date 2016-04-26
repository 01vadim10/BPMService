using System;

namespace BPMService.WebUI.Models
{
    public class ContactView
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string MobilePhone { get; set; }

        public string Dear { get; set; }

        public string JobTitle { get; set; }

        public DateTime? BirthDate { get; set; }
    }
}