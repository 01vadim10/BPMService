using System;
using System.Collections.Generic;
using System.Data.Services.Client;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BPMService.WebUI.Models;
using BPMService.WebUI.BPMonlineSvcRef;

namespace BPMService.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private IEnumerable<Contact> contactsApi;

        //public HomeController(IEnumerable<Contact> contacts)
        //{
        //    contactsApi = contacts;
        //}

        public ActionResult Index()
        {
            if (LoginClass.TryLogin())
            {
                //ViewBag.Contacts = EDProxyService.GetOdataCollection();
                contactsApi = EDProxyService.GetOdataCollection(180);
                return View(contactsApi);
            }
            return View();
        }

        public ViewResult Create()
        {
            return View();
        }

        public ViewResult Edit(Guid contactId)
        {
            Contact contact = contactsApi.FirstOrDefault(p => p.Id == contactId);
            return View(contact);
        }

        public void Delete(Guid contactId)
        {
            EDProxyService.DeleteContact(contactId);
        }
	}
}