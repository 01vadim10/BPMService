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

        public HomeController()
        {
            if (LoginClass.TryLogin())
            {
                //ViewBag.Contacts = EDProxyService.GetOdataCollection();
                contactsApi = EDProxyService.GetOdataCollection();
            }
        }

        public ActionResult Index()
        {
            //if (LoginClass.TryLogin())
            //{
            //    //ViewBag.Contacts = EDProxyService.GetOdataCollection();
            //    this.contactsApi = EDProxyService.GetOdataCollection(180);
            ////    return View(contactsApi);
            //}
            return View(contactsApi);
        }

        public ViewResult Create()
        {
            return View(new Contact());
        }

        [HttpPost]
        public ActionResult Create(Contact newContact)
        {
            if (ModelState.IsValid)
            {
                EDProxyService.CreateContact(newContact);
                TempData["message"] = string.Format("Контакт {0} был создан", newContact.Name);
                return RedirectToAction("Index");
            }
            else
            {
                return View(newContact);
            }

        }

        public ViewResult Edit(Guid contactId)
        {
            Contact contact = contactsApi.FirstOrDefault(p => p.Id == contactId);
            return View(contact);
        }

        [HttpPost]
        public ActionResult Save(Contact contact)
        {
            if (ModelState.IsValid)
            {
                EDProxyService.UpdateContact(contact);
                TempData["message"] = string.Format("Изменения контакта {0} были сохранены", contact.Name);
                return RedirectToAction("Index");
            }
            else
            {
                return View(contact);
            }
        }

        public ActionResult Delete(Guid contactId)
        {
            if (ModelState.IsValid)
            {
                EDProxyService.DeleteContact(contactId);
                TempData["message"] = "Удаление контакта было произведено";
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }
	}
}