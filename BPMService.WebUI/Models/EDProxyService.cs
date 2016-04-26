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

        public static IEnumerable<Contact> GetOdataCollection(int skip = 0)
        {
            //Создание массива контактов
            IEnumerable<Contact> allContacts = null;
            // Создание контекста приложения BPMonline.
            var context = new BPMonline(serverUri);
            // Определение метода, который добавляет аутентификационные cookie при создании нового запроса.
            context.SendingRequest += new EventHandler<SendingRequestEventArgs>(OnSendingRequestCookie);
            try
            {
                // Построение запроса LINQ для получение коллекции контактов.
                allContacts = from contacts in context.ContactCollection.Skip(skip)
                                  select contacts;
            }
            catch (Exception ex)
            {
                // Обработка ошибок.
            }
            return allContacts;
        }
    }
}