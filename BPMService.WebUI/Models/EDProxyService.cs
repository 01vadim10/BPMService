using System;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using System.Data.Services.Client;
using BPMService.WebUI.BPMonlineSvcRef;

namespace BPMService.WebUI.Models
{
    public class EDProxyService
    {
        //Объявление переменной адреса сервиса OData
        private static Uri serverUri = new Uri("http://178.159.246.209:1410/0/ServiceModel/EntityDataService.svc");

        public static void OnSendingRequestCookie(object sender, SendingRequestEventArgs e)
        {
            //Вызов метода класса LoginClass, реализующего аутентификацию переданного в параметрах метода пользователя.
            LoginClass.TryLogin();
            var req = e.Request as HttpWebRequest;
            //Добавление полученных аутентификационных cookie в запрос на получение данных.
            req.CookieContainer = LoginClass.AuthCookie;
            e.Request = req;
        }

        public static ContactView[] GetOdataCollection()
        {
            //Создание массива контактов
            ContactView[] contactsView = {};
            // Создание контекста приложения BPMonline.
            var context = new BPMonline(serverUri);
            // Определение метода, который добавляет аутентификационные cookie при создании нового запроса.
            context.SendingRequest += new EventHandler<SendingRequestEventArgs>(OnSendingRequestCookie);
            try
            {
                // Построение запроса LINQ для получение коллекции контактов.
                var allContacts = from contacts in context.ContactCollection
                                  select new
                                  {
                                      contacts.BirthDate,
                                      contacts.Dear,
                                      contacts.JobTitle,
                                      contacts.MobilePhone,
                                      contacts.Name
                                  };
                //return allContacts;
                int i = 0;
                foreach(var contact in allContacts)
                {
                    contactsView[i].Name = (string) contact.Name;
                    contactsView[i].BirthDate = contact.BirthDate;
                    contactsView[i].Dear = contact.Dear;
                    contactsView[i].JobTitle = contact.JobTitle;
                    contactsView[i++].MobilePhone = contact.MobilePhone;
                }
            }
            catch (Exception ex)
            {
                // Обработка ошибок.
            }
            return contactsView;
        }
    }
}