using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;

namespace App_Crawler
{
    public partial class Form1 : Form
    {
        static List<Book> books = new List<Book>();
        public Form1()
        {
            InitializeComponent();


        }

        class Person
        {
            public string Name { get; set; }
            public string Surname { get; set; }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            startCrawler();
            foreach (var book in books)
            {
                dataGridView1.Rows.Add(book.Name, book.Url);
            }
        }

        private static void startCrawler()
        {
            var url = "https://webtruyen.com/";
            HtmlWeb htmlWeb = new HtmlWeb()
            {
                AutoDetectEncoding = false,
                OverrideEncoding = Encoding.UTF8
            };
            HtmlDocument htmlDocument = htmlWeb.Load(url);

            var listLiCategories = htmlDocument.DocumentNode.Descendants("div").First(node => node.GetAttributeValue("Class", "").Equals("sidebar-content"))
                .ChildNodes.First(child => child.Name == "ul").ChildNodes.Where(n => n.Name == "li").ToList();
            foreach (var category in listLiCategories)
            {
                HtmlNode nodeHref;
                if (category.ChildNodes.Any(node => node.Name == "b"))
                {
                    nodeHref = category.Descendants("b").First().ChildNodes.First(child => child.Name == "a");
                }
                else
                {
                    nodeHref = category.Descendants("a").First();
                }
                var name = category.InnerText;
                var link = nodeHref.Attributes["href"].Value;
                books.Add(new Book() { Name = name, Url = link });
            }
        }
    }
}
