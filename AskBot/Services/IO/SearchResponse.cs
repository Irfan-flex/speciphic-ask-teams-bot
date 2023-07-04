using System;
using System.Collections.Generic;
using System.Linq;

namespace AskBot.Services.IO
{
    public class SearchResponse
    {
        public string Answer { get; set; }
        public string Context { get; set; }
        public List<Metum> Meta { get; set; }
        public float? Score { get; set; }

        public string GetFilesSummary(int showfilesCount)
        {
            return string.Join(Environment.NewLine, this.Meta.Take(showfilesCount).Select(x => x.ToHtmlString()));
        }
    }
}
