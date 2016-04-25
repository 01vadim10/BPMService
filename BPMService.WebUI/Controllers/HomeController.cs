using System;
using System.Data.Services.Client;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BPMService.WebUI.Models;

namespace BPMService.WebUI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (LoginClass.TryLogin("Пользователь 1", "Пользователь 1"))
            {
                EDProxyService.GetOdataCollection();
            }
            return View();
        }
	}
}