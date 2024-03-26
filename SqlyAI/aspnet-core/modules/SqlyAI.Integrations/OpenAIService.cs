using AppCommon.Enums;
using Newtonsoft.Json;
using SqlyAI.Integrations.Dtos.OpenAI;
using SqlyAI.Integrations.Interfaces;
using System.Net.Http.Headers;
using System.Text;

namespace SqlyAI.Integrations
{
    public class OpenAIService : IOpenAIService
    {
        //public OpenAIService(IAppIntegrationConfigurationAccessor configuration)
        //{

        //}


        public async Task<CompletionOutput> TextToSql(DbType dbType, string text, IEnumerable<TableInfo> tableInfoes)
        {
            var dbTypeStr = (dbType == DbType.CSV || dbType == DbType.GoogleSheet) ? DbType.SQLServer.ToString() : dbType.ToString();
            var sb = new StringBuilder();
            foreach (var tableInfo in tableInfoes)
            {
                sb.Append("# ");
                sb.Append(tableInfo.TableName);
                sb.Append("(");
                sb.Append(string.Join(",", tableInfo.Columns));
                sb.AppendLine(")");
            }

            var query = $$"""
                ### {{dbTypeStr}} tables, with their properties:
                #
                {{sb}}
                #
                ### Create a {{dbTypeStr}} query to {{text}}
                SELECT
                """;

            var request = new OpenAIRequestInput("text-davinci-003", query, 0, 500, 1, 0, 0, new List<string> { "#", ";" });
            var result = await SendCompletionAsync(request);
            return JsonConvert.DeserializeObject<CompletionOutput>(result);

        }

        public async Task<string> SendCompletionAsync(OpenAIRequestInput input)
        {
            var token = "token";
            using var httpClient = new HttpClient();
            using var request = new HttpRequestMessage(new HttpMethod("POST"), "https://api.openai.com/v1/completions");
            request.Headers.TryAddWithoutValidation("Authorization", $"Bearer {token}");
            request.Content = new StringContent(JsonConvert.SerializeObject(input));

            request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

            var response = await httpClient.SendAsync(request);
            return await response.Content.ReadAsStringAsync();
        }
    }

    public record TableInfo(string TableName, IEnumerable<string> Columns);
    public record OpenAIRequestInput(string model, string prompt, int temperature, int max_tokens, int top_p, int frequency_penalty, int presence_penalty, List<string> stop);
}