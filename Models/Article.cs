using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace WebChangeAlarm.Models
{
    public class Article : IEquatable<Article>
    {

        public string Title { get; }
        public DateTime TimeStamp { get; }
        public string Content { get; }

        public HtmlNode HtmlTitle { get; }
        public HtmlNode HtmlDateTime { get; }
        public HtmlNode HtmlPost { get; }


        public Article(HtmlNode t, HtmlNode dt, HtmlNode c)
        {
            HtmlTitle = t;
            HtmlDateTime = dt;
            HtmlPost = c;

            Title = t.InnerText;
            TimeStamp = DateTime.Parse(dt.InnerText);
            Content = c.InnerText;
        }

        public Article(string loggedLine)
        {

            string[] parts = loggedLine.Split(" /// ");

            if (parts.Length == 2)
            {
                Title = parts[1];
                TimeStamp = DateTime.Parse(parts[0]);
                Content = null;
            }

        }

        public Article(Article art)
        {
            HtmlTitle = art.HtmlTitle;
            HtmlDateTime = art.HtmlDateTime;
            HtmlPost = art.HtmlPost;

            Title = art.Title;
            TimeStamp = art.TimeStamp;
            Content = art.Content;
        }

        public override string ToString()
        {
            return string.Format("{0} /// {1}", TimeStamp, Title);
        }

        public bool Equals([AllowNull] Article other)
        {
            if (this.Title == other.Title && this.TimeStamp == other.TimeStamp)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
