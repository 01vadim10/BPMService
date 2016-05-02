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
        // Создание контекста приложения BPMonline.
        private static BPMonline context = new BPMonline(serverUri);

        public static void OnSendingRequestCookie(object sender, SendingRequestEventArgs e)
        {
            //Вызов метода класса LoginClass, реализующего аутентификацию переданного в параметрах метода пользователя.
            LoginClass.TryLogin();
            
            var req = e.Request as HttpWebRequest;
            //Добавление полученных аутентификационных cookie в запрос на получение данных.
            req.CookieContainer = LoginClass.AuthCookie;
            e.Request = req;
        }

        public static IEnumerable<Contact> GetOdataCollection(int take = 40)
        {
            //Создание массива контактов
            IEnumerable<Contact> allContacts = null;
            // Определение метода, который добавляет аутентификационные cookie при создании нового запроса.
            context.SendingRequest += new EventHandler<SendingRequestEventArgs>(OnSendingRequestCookie);

            try
            {
                // Построение запроса LINQ для получение коллекции контактов.
                allContacts = (from contacts in context.ContactCollection.OrderByDescending(p => p.CreatedOn)
                                  select contacts).Take(take);
            }
            catch (Exception ex)
            {
                // Обработка ошибок.
            }
            return allContacts;
        }

        //Создание нового объекта
        public static void CreateContact(Contact contact/*, Account account*/)
        {
            // Определение метода, который добавляет аутентификационные cookie при создании нового запроса.
            context.SendingRequest += new EventHandler<SendingRequestEventArgs>(OnSendingRequestCookie);
            // Добавление созданного контакта в коллекцию контактов модели данных сервиса.
            context.AddToContactCollection(contact);
            // Сохранение изменений данных в BPMonline одним запросом.
            DataServiceResponse responces = context.SaveChanges(SaveChangesOptions.Batch);
            // Обработка ответов от сервера.
        }

        //Изменение существующего объекта
        public static void UpdateContact(Contact contact)
        {
            // Определение метода, который добавляет аутентификационные cookie при создании нового запроса.
            context.SendingRequest += new EventHandler<SendingRequestEventArgs>(OnSendingRequestCookie);
            // Из колллекции контактов выбирается тот, по которому будет изменяться информация.
            var updateContact = context.ContactCollection.Where(c => c.Id.Equals(contact.Id)).First();
            // Изменение свойств выбранного контакта.
            updateContact.Name = contact.Name;
            updateContact.Dear = contact.Dear;
            updateContact.MobilePhone = contact.MobilePhone;
            updateContact.JobTitle = contact.JobTitle;
            updateContact.BirthDate = contact.BirthDate;
            // Сохранение изменений в модели данных сервиса.
            context.UpdateObject(updateContact);
            // Сохранение изменений данных в BPMonline одним запросом.
            var responces = context.SaveChanges(SaveChangesOptions.Batch);
        }

        //Удаление объекта
        public static void DeleteContact(Guid contactId)
        {
            context.SendingRequest += new EventHandler<SendingRequestEventArgs>(OnSendingRequestCookie);
            // Из коллекции контактов выбирается тот объект, который будет удален.
            var deleteContact = context.ContactCollection.Where(c => c.Id.Equals(contactId)).First();
            // Удаление выбранного объекта из модели данных сервиса.
            context.DeleteObject(deleteContact);
            // Сохранение изменений данных в BPMonline одним запросом.
            var responces = context.SaveChanges(SaveChangesOptions.Batch);
            // ОБработка ответов от сервера.
        }
    }
}