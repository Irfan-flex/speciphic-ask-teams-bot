using System;

namespace AskBot.Services.IO
{
    public class Metum
    {
        public string FileName { get; set; }
        public string OriginalFileName { get; set; }
        public int Page { get; set; }
        public string Url { get; set; }

        public string ToHtmlString()
        {
            return $"{Environment.NewLine}<a href =\"{Url}\"> {OriginalFileName ?? FileName} </a> See page no. {Page}{Environment.NewLine}";
        }
    }
}
