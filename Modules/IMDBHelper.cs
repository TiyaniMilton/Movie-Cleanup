using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;
using System.Net;
using mshtml;
using System.Windows.Browser;
using System.Xml;

namespace Movie_Cleanup.Modules
{
    public class IMDBHelper
    {

        public static imdbitem GetInfoByTitle(string Title)
        {
            string url = "http://imdbapi.org/?type=xml&limit=1&title=" + Title;
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);
            req.Method = "GET";
            req.UserAgent = "Mozilla/5.0 (Windows; U; MSIE 9.0; WIndows NT 9.0; en-US))";
            string source;
            using (StreamReader reader = new StreamReader(req.GetResponse().GetResponseStream()))
            {
                source = reader.ReadToEnd();
            }
            //HtmlDocument doc = new HtmlDocument();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(source);
            
            //doc.LoadHtml(source);
            //XDocument xdoc = XDocument.Parse(doc.DocumentNode.InnerHtml, LoadOptions.None);
            XDocument xdoc = XDocument.Parse(doc.DocumentElement.InnerXml, LoadOptions.None);
            imdbitem i = new imdbitem();
            i.rating = xdoc.Descendants("rating").Select(x => x.Value).FirstOrDefault();
            i.rating_count = xdoc.Descendants("rating_count").Select(x => x.Value).FirstOrDefault();
            i.year = xdoc.Descendants("year").Select(x => x.Value).FirstOrDefault();
            i.rated = xdoc.Descendants("rated").Select(x => x.Value).FirstOrDefault();
            i.title = xdoc.Descendants("title").Select(x => x.Value).FirstOrDefault();
            i.imdb_url = xdoc.Descendants("imdb_url").Select(x => x.Value).FirstOrDefault();
            i.plot_simple = xdoc.Descendants("plot_simple").Select(x => x.Value).FirstOrDefault();
            i.type = xdoc.Descendants("type").Select(x => x.Value).FirstOrDefault();
            i.poster = xdoc.Descendants("poster").Select(x => x.Value).FirstOrDefault();
            i.imdb_id = xdoc.Descendants("imdb_id").Select(x => x.Value).FirstOrDefault();
            i.also_known_as = xdoc.Descendants("also_known_as").Select(x => x.Value).FirstOrDefault();
            i.language = xdoc.Descendants("language").Select(x => x.Value).FirstOrDefault();
            i.country = xdoc.Descendants("country").Select(x => x.Value).FirstOrDefault();
            i.release_date = xdoc.Descendants("release_date").Select(x => x.Value).FirstOrDefault();
            i.filming_locations = xdoc.Descendants("filming_locations").Select(x => x.Value).FirstOrDefault();
            i.runtime = xdoc.Descendants("runtime").Select(x => x.Value).FirstOrDefault();
            i.directors = xdoc.Descendants("directors").Descendants("item").Select(x => x.Value).ToList();
            i.writers = xdoc.Descendants("writers").Descendants("item").Select(x => x.Value).ToList();
            i.actors = xdoc.Descendants("actors").Descendants("item").Select(x => x.Value).ToList();
            i.genres = xdoc.Descendants("genres").Descendants("item").Select(x => x.Value).ToList();
            return i;
        }

        public class imdbitem
        {
            public string rating { get; set; }
            public string rating_count { get; set; }
            public string year { get; set; }
            public string rated { get; set; }
            public string title { get; set; }
            public string imdb_url { get; set; }
            public string plot_simple { get; set; }
            public string type { get; set; }
            public string poster { get; set; }
            public string imdb_id { get; set; }
            public string also_known_as { get; set; }
            public string language { get; set; }
            public string country { get; set; }
            public string release_date { get; set; }
            public string filming_locations { get; set; }
            public string runtime { get; set; }
            public List<string> directors { get; set; }
            public List<string> writers { get; set; }
            public List<string> actors { get; set; }
            public List<string> genres { get; set; }
        }

    }
}
