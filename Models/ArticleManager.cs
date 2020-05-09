using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace WebChangeAlarm.Models
{
    public class ArticleManager
    {
        private string logfile;

        public List<Article> LoggedArticles { get; }

        public ArticleManager(string filename)
        {
            logfile = filename;
            LoggedArticles = new List<Article>();
        }


        public void ReadLoggedArticles()
        {
            using (StreamReader sr = new StreamReader(logfile))
            {
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    if (line != null && line != "") LoggedArticles.Add(new Article(line));
                }
            }
        }

        public void SaveLoggedArticles(List<Article> publishedArticles)
        {
            using (StreamWriter sw = new StreamWriter(logfile))
            {
                foreach (Article pubArt in publishedArticles)
                {
                    sw.WriteLine(pubArt.ToString());
                }
            }
        }

        public List<Article> CompareArticleAndGetNewArticles(List<Article> publishedArticles)
        {
            List<Article> newArticles = new List<Article>();

            foreach (Article pubArt in publishedArticles)
            {
                if (!LoggedArticles.Contains(pubArt)) newArticles.Add(new Article(pubArt));
            }

            return newArticles;

        }

    }
}
