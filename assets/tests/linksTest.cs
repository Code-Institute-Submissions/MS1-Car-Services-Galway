using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Net;

namespace ConsoleAppTest1
{
    class Program
    {
        static void Main(string[] args)
        {
                        //Below you can place link to your website
            var html = @"https://kpokc.github.io/MS1-Car-Services-Galway/index.html";

            HtmlWeb web = new HtmlWeb();

            var htmlDoc = web.Load(html);

                            //HAP is going to retrieve all "a" tags from DOM
            var htmlNodes = htmlDoc.DocumentNode.SelectNodes("//*/a");

            List<string> urlList = new List<string>();

            foreach (var node in htmlNodes)
            {
                //standard website Navigation links (/index.html, /aboutme.html ...)
                if (node.Attributes["href"].Value.Substring(0, 1) == "/")
                {
                    urlList.Add(html.Substring(0, html.Length - node.Attributes["href"].Value.Length) + node.Attributes["href"].Value);
                }
                //section navigation links (#home, #abouut_us ...)
                else if (node.Attributes["href"].Value.Substring(0, 1) == "#")
                {
                    urlList.Add(html + node.Attributes["href"].Value);
                }
                else
                {    //third party links
                    urlList.Add(node.Attributes["href"].Value);
                }

            }
            //remove duplicates from list
            urlList = urlList.Distinct().ToList();

            foreach (string link in urlList)
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(link);
                
                try
                {
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                            //Get status of response
                    String ver = response.StatusCode.ToString();
                    if (ver == "OK")
                    {
                        /*var status = response.StatusCode;*/
                        Console.WriteLine(link + "   " + ver);
                        response.Close();
                    }
                    else
                    {
                        Console.WriteLine(link + "   " + "Broken");
                        response.Close();
                    }
                }
                catch (Exception error)
                {           //When link is broken
                    Console.WriteLine(link + "   " + error.Message);
                }

            }

            Console.ReadLine();
        }

    }
}
