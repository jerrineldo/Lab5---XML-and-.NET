using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using XmlTest.Models;

namespace XmlTest.Controllers
{
    public class BookController : Controller
    {
        public IActionResult Index()
        {
            IList<Book> bookList = new List<Book>();

            //Loading the books.xml file
            string path = Request.PathBase + "App_Data/books.xml";

            XmlDocument doc = new XmlDocument();

            if (System.IO.File.Exists(path))
            {
                doc.Load(path);
                XmlNodeList books = doc.GetElementsByTagName("book");
                foreach (XmlElement b in books)
                {
                    Book NewBook = new Book();
                    NewBook.bookId =  Int32.Parse(b.GetElementsByTagName("id")[0].InnerText);
                    NewBook.bookTitle = b.GetElementsByTagName("title")[0].InnerText;
                    NewBook.authorfName = b.GetElementsByTagName("firstname")[0].InnerText;
                    NewBook.authorlName = b.GetElementsByTagName("lastname")[0].InnerText;
                    bookList.Add(NewBook);
                }
            }
            else
            {

            }

            return View(bookList);
        }

        [HttpGet]
        public IActionResult Create()
        {
            Book NewBook = new Book();
            return View(NewBook);
        }

        [HttpPost]
        public IActionResult Create(string bookTitle,string authorfName, string authorlName)
        {
            Book b = new Book();
            b.bookTitle = bookTitle;
            b.authorfName = authorfName;
            b.authorlName = authorlName;

            //Loading the books.xml file
            string path = Request.PathBase + "App_Data/books.xml";

            XmlDocument doc = new XmlDocument();
            if (System.IO.File.Exists(path))
            {
                doc.Load(path);

                XmlNodeList books = doc.GetElementsByTagName("book");

                //Removing the oldest record to always fit in 5 books alone.
                if (books.Count >= 5)
                {
                    doc.DocumentElement.RemoveChild(books[0]);
                }

                //create a new book

                XmlElement book = _CreateBookElement(doc, b);

                //get the root element
                doc.DocumentElement.AppendChild(book);

            }
            else
            {
                //file doesnt exist, so create a new person
                XmlNode dec = doc.CreateXmlDeclaration("1.0", "utf-8", "");
                doc.AppendChild(dec);
                XmlNode root = doc.CreateElement("books");

                //create a new person
                XmlElement book = _CreateBookElement(doc, b);
                root.AppendChild(book);
                doc.AppendChild(root);

            }
            doc.Save(path);
            return RedirectToAction("Index");
        }
        private XmlElement _CreateBookElement(XmlDocument doc, Book newBook)
        {
            XmlElement book = doc.CreateElement("book");

            //getting the id of the last book.
            var lastbookId = doc.DocumentElement.LastChild.FirstChild.InnerText;

            XmlNode id = doc.CreateElement("id");
            id.InnerText = (Int32.Parse(lastbookId) + 1).ToString();
            XmlNode Title = doc.CreateElement("title");
            Title.InnerText = newBook.bookTitle;
            XmlNode author = doc.CreateElement("author");
            XmlNode first = doc.CreateElement("firstname");
            first.InnerText = newBook.authorfName;
            XmlNode last = doc.CreateElement("lastname");
            last.InnerText = newBook.authorlName;
            
            author.AppendChild(first);
            author.AppendChild(last);

            book.AppendChild(id);
            book.AppendChild(author);
            book.AppendChild(Title);
     
            return book;
        }
    }
}
