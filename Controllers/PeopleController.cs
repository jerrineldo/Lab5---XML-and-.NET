using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace XmlTest.Controllers
{
    public class PeopleController : Controller
    {
        public IActionResult Index()
        {
            IList<Models.Person> personList = new List<Models.Person>();

            //load the people.xml file
            string path = Request.PathBase + "App_Data/people.xml";
            XmlDocument doc = new XmlDocument();

            if (System.IO.File.Exists(path))
            {
                doc.Load(path);
                XmlNodeList people = doc.GetElementsByTagName("person");
                foreach (XmlElement p in people)
                {
                    Models.Person person = new Models.Person();
                    person.Job = p.GetAttribute("job");
                    person.FirstName = p.GetElementsByTagName("first")[0].InnerText;
                    person.LastName = p.GetElementsByTagName("last")[0].InnerText;
                    personList.Add(person);
                }

            }
            return View(personList);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var person = new Models.Person();
            return View(person);
        }

        [HttpPost]
        public IActionResult Create(Models.Person p)
        {
            //load the people.xml file
            string path = Request.PathBase + "App_Data/people.xml";
            XmlDocument doc = new XmlDocument();

            if (System.IO.File.Exists(path))
            {
                //If the file exists , just load it and create new person
                doc.Load(path);

                //create a new person

                XmlElement person = _CreatePersonElement(doc, p);

                //get the root element
                doc.DocumentElement.AppendChild(person);
            }
            else
            {
                //file doesnt exist, so create a new person
                XmlNode dec = doc.CreateXmlDeclaration("1.0", "utf-8", "");
                doc.AppendChild(dec);
                XmlNode root = doc.CreateElement("people");

                //create a new person
                XmlElement person = _CreatePersonElement(doc, p);
                root.AppendChild(person);
                doc.AppendChild(root);
            }
            doc.Save(path);
            return View();
        }
        private XmlElement _CreatePersonElement(XmlDocument doc, Models.Person newPerson)
        {
            XmlElement person = doc.CreateElement("person");
            XmlAttribute job = doc.CreateAttribute("job");
            job.Value = newPerson.Job;
            person.Attributes.Append(job);

            XmlNode name = doc.CreateElement("name");
            XmlNode first = doc.CreateElement("first");
            first.InnerText = newPerson.FirstName;
            XmlNode last = doc.CreateElement("last");
            last.InnerText = newPerson.LastName;

            name.AppendChild(first);
            name.AppendChild(last);

            person.AppendChild(name);

            return person;
        }
    }
}
