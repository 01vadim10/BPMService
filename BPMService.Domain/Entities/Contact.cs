using System;
using System.Data.Services.Client;
using System.Linq;
using System.Net;
using BPMService.Domain.BPMonlineSvcRef;

namespace BPMService.Domain.Entities
{
    public class Contact
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string MPhone { get; set; }

        public string Dear { get; set; }

        public string JobTitle { get; set; }

        public DateTime BirthDate { get; set; }

        
    }
}