﻿using System;
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
                contactsApi = EDProxyService.GetOdataCollection(180);
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
            return View();
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
                EDProxyService.UpdateContact(/*contact*/);
                TempData["message"] = string.Format("Изменения контакта {0} были сохранены", contact.Name);
                return RedirectToAction("Index");
            }
            else
            {
                return View(contact);
            }
        }

        public void Delete(Guid contactId)
        {
            EDProxyService.DeleteContact(contactId);
        }
	}
}