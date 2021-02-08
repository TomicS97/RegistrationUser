using Registracija_SrdjanTomic.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;

namespace Registracija_SrdjanTomic.Controllers
{
    public class RegistrationController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetUser()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(Server.MapPath("~/RegistrationXML/data.xml"));
            List<Users> users = new List<Users>();

            foreach (XmlNode node in doc.SelectNodes("/data/continent/country/user"))
            {
                users.Add(new Users
                {
                    FirstName = node["first_name"].InnerText,
                    LastName = node["last_name"].InnerText,
                    Address = node["address"].InnerText,
                    City = node["city"].InnerText,
                    Country = node.ParentNode.Attributes[0].Value,
                    Email = node["email"].InnerText
                });
            }

            var data = users;
            return Json(data, JsonRequestBehavior.AllowGet);
        }


        public ActionResult SaveNewUser(string firstName, string lastname, string address, string city, string country, string email, string continent)
        {

            XmlDocument xmlUserDoc = new XmlDocument();
            xmlUserDoc.Load(Server.MapPath("~/RegistrationXML/data.xml"));

            XmlElement ParentElement = xmlUserDoc.CreateElement("user");

            XmlElement FirstName = xmlUserDoc.CreateElement("first_name");
            FirstName.InnerText = firstName;
            XmlElement LastName = xmlUserDoc.CreateElement("last_name");
            LastName.InnerText = lastname;
            XmlElement Address = xmlUserDoc.CreateElement("address");
            Address.InnerText = address;
            XmlElement City = xmlUserDoc.CreateElement("city");
            City.InnerText = city;
            XmlElement Email = xmlUserDoc.CreateElement("email");
            Email.InnerText = email;

            ParentElement.AppendChild(FirstName);
            ParentElement.AppendChild(LastName);
            ParentElement.AppendChild(Address);
            ParentElement.AppendChild(City);
            ParentElement.AppendChild(Email);

            XmlNodeList nodes = xmlUserDoc.SelectNodes("//country");
            foreach (XmlNode node in nodes)
            {
                var name = node.Attributes["name"].Value;

                if (name == country)
                {
                    node.AppendChild(ParentElement);
                    xmlUserDoc.Save(Server.MapPath("~/RegistrationXML/data.xml"));

                    return Json(data: xmlUserDoc);
                }
            }

            nodes = xmlUserDoc.SelectNodes("//continent");
            foreach (XmlNode item in nodes)
            {
                var name = item.Attributes["name"].Value;
                if (name == continent)
                {
                    XmlElement cont = xmlUserDoc.CreateElement("country");
                    cont.InnerText = country;

                    cont.AppendChild(ParentElement);
                    xmlUserDoc.DocumentElement.AppendChild(cont);
                    xmlUserDoc.Save(Server.MapPath("~/RegistrationXML/data.xml"));

                    return Json(data: xmlUserDoc);
                }

            }
            XmlElement conti = xmlUserDoc.CreateElement("continent");
            conti.InnerText = continent;

            XmlElement country2 = xmlUserDoc.CreateElement("country");
            country2.InnerText = country;

            country2.AppendChild(ParentElement);
            conti.AppendChild(country2);
            xmlUserDoc.DocumentElement.AppendChild(conti);

            xmlUserDoc.Save(Server.MapPath("~/RegistrationXML/data.xml"));

            return Json(data: xmlUserDoc);
        }


        public ActionResult DeleteUser(string email)
        {
            XDocument xmlDoc = XDocument.Load(Server.MapPath("~/RegistrationXML/data.xml"));
            var items = (from item in xmlDoc.Descendants("user") select item).ToList();
            XElement selected = items.Where(p => p.Element("email").Value == email.ToString()).FirstOrDefault();
            selected.Remove();
            xmlDoc.Save(Server.MapPath("~/RegistrationXML/data.xml"));

            return RedirectToAction("Index", "Registration");

        }
    }
}