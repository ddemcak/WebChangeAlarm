using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using HtmlAgilityPack;
using MailKit.Net.Smtp;
using MimeKit;
using WebChangeAlarm.Configurators;
using WebChangeAlarm.Models;


namespace WebChangeAlarm
{
    public class WebChangeAlarm
    {

        #region CONSTATNS
        
        private const string _LastEntries = "recentPublishedEntries.txt";
        private const string _LastCheck = "lastCheck.txt";

        #endregion


        #region PROPERTIES

        private static ConfigurationFileManager cfm;
        private static ArticleManager artManager = new ArticleManager(_LastEntries);
        
        private static List<Article> publishedArticles = new List<Article>();
        private static List<string> recipients = new List<string>();

        #endregion



        static void Main(string[] args)
        {

            if (args.Length != 1)
            {
                Console.WriteLine("Please provide config.ini file as parameter!");
                Console.WriteLine("Exiting application...");
                return;
            }


            string configFile = args[0];
            

            if (!File.Exists(configFile))
            {
                Console.WriteLine("Configuration file does not exist! Exiting application...");
                return;
            }
            else
            {
                cfm = new ConfigurationFileManager(configFile);
            }


            if (cfm.EmailRecipentsSourceType == "url") LoadRecipientsFromWeb(cfm.EmailRecipentsSource);
            else LoadRecipients(cfm.EmailRecipentsSource);


            if (!File.Exists(_LastEntries)) File.Create(_LastEntries).Dispose();


            WebClient wc = new WebClient();
            string content = wc.DownloadString(cfm.WebsiteUrl);


            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(content);

            var titles = doc.DocumentNode.SelectNodes("//h2[contains(@class, 'art-postheader')]");
            var datetimes = doc.DocumentNode.SelectNodes("//span[contains(@class, 'art-postdateicon')]");
            var posts = doc.DocumentNode.SelectNodes("//div[contains(@class, 'art-article')]");


            if (titles.Count == datetimes.Count && datetimes.Count == posts.Count)
            {
                for (int i = 0; i < titles.Count; i++)
                {
                    publishedArticles.Add(new Article(titles[i], datetimes[i], posts[i]));
                }
            }

            artManager.ReadLoggedArticles();
            List<Article> newArts = artManager.CompareArticleAndGetNewArticles(publishedArticles);

            if (newArts.Count != 0)
            {
                artManager.SaveLoggedArticles(publishedArticles);

                foreach (Article newArt in newArts) SendEmail(newArt.HtmlTitle, newArt.HtmlPost);

                Console.WriteLine(string.Format("{0} new article(s) found. Email(s) sent.", newArts.Count));
                UpdateCheckedAt(string.Format("{0} new article(s) found. Email(s) sent.", newArts.Count));
            }
            else
            {
                Console.WriteLine("No new articles found.");
                UpdateCheckedAt("No new articles found.");
            }

        }


        private static void UpdateCheckedAt(string messageLog)
        {
            using (StreamWriter sw = new StreamWriter(_LastCheck, true))
            {
                sw.WriteLine(DateTime.Now + " - " + messageLog);
            }
        }

        private static void LoadRecipients(string filename)
        {
            using (StreamReader sr = new StreamReader(filename))
            {
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    if (line != null && line != "") recipients.Add(line);
                }
            }
        }

        private static void LoadRecipientsFromWeb(string url)
        {
            WebClient wc = new WebClient();
            string content = wc.DownloadString(url);

            foreach (string email in content.Split('\n'))
            {
                if (email != null && email != "") recipients.Add(email);
            }

        }


        private static void SendEmail(HtmlNode title, HtmlNode post)
        {
            MimeMessage message = new MimeMessage();

            MailboxAddress from = new MailboxAddress(cfm.EmailFromName, cfm.EmailFromEmail);
            message.From.Add(from);

            MailboxAddress to = new MailboxAddress(cfm.EmailAdminEmail);
            message.To.Add(to);

            message.Subject = "[Novinky]: " + title.InnerText;

            BodyBuilder bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = string.Format("{0}<p>Zdroj: <a href=\"{1}\">{1}</a><br><a href=\"{2}\">Odhlásit</a> z odběru.</p>", post.InnerHtml, cfm.WebsiteUrl, cfm.WebsiteHomeUrl);
            bodyBuilder.TextBody = string.Format("{0}\n\nZdroj: {1}\nOdhlásit z odběru na: {2}", post.InnerText, cfm.WebsiteUrl, cfm.WebsiteHomeUrl);

            message.Body = bodyBuilder.ToMessageBody();

            SmtpClient client = new SmtpClient();
            client.Connect(cfm.SmtpClientHost, cfm.SmtpClientPort, cfm.SmtpClientUseSsl);
            client.Authenticate(cfm.SmtpClientUsername, cfm.SmtpClientPassword);

            // Send email notification to ADMIN
            client.Send(message);

            // Send email for all recepients
            foreach (string recipient in recipients)
            {
                message.To.Clear();

                to = new MailboxAddress(recipient);
                message.To.Add(to);

                client.Send(message);
            }

            client.Disconnect(true);
            client.Dispose();
        }

    }
}
