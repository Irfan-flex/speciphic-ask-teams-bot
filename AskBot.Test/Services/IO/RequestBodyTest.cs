using AskBot.Services.IO;
using System.Diagnostics;
using System.Text.Json;

namespace AskBot.Test.Services.IO
{
    [TestClass]
    public class RequestBodyTest
    {
        private readonly SearchRequestBody _searchRequestBody;

        public RequestBodyTest()
        {
            _searchRequestBody = new SearchRequestBody()
            {
                AcceptLanguage = "en-US",
            };
        }

        [TestMethod]
        public void ToString_ConvertsToJson()
        {
            string result = _searchRequestBody.ToString();
            var expectedOutput = JsonSerializer.Serialize(new
            {
                acceptLanguage = "en-US"
            });

            Assert.AreEqual(expectedOutput, result);
        }
    }
}