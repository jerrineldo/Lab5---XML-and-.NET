using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Xsl;
using System.IO; //to be able to create a stream.

namespace XmlTest.Controllers
{
    public class SchoolController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(string Type)
        {

            //load the xml file
            XmlDocument xml = new XmlDocument();
            xml.Load(Request.PathBase + "App_Data/schools.xml");

            //load the XSLT
            XslCompiledTransform xsl = new XslCompiledTransform();
            xml.Load(Request.PathBase + "App_Data/schools-filtered.xsl");
            XsltArgumentList args = new XsltArgumentList();
            args.AddParam("schooltype", "", Type);

            StringWriter str = new StringWriter();
            xsl.Transform(xml, args, str);

            ViewBag.output = str.ToString();
            //pass the parameter value to XSLT
            return View();
        }
    }
}
