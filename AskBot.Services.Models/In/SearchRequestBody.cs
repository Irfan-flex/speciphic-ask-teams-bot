namespace AskBot.Services.Models.In
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
