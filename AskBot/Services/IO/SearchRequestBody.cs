namespace AskBot.Services.IO
{
    public class SearchRequestBody
    {
        public string AcceptLanguage { get; set; }

        public override string ToString()
        {
            return JsonConverter.ToString(this);
        }

    }
}
