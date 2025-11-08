using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using knowledgeBase.Entities;
using knowledgeBase.Repositories;

namespace knowledgeBase.Services;

public class AiService
{
    public HttpClient Client { get; set; }
    private QuestionLogRepository _questionLogRepository;
    private ArticleRepository _articleRepository;
    private ArticleService _articleService;

    public async Task<string> GetAnswer(string question)
    {
        try
        {
            var requestData = new
            {
                model = "deepseek/deepseek-chat-v3-0324",
                messages = new[]
                {
                    new
                    {
                        role = "user",
                        content = question
                    }
                }
            };

            string json = JsonSerializer.Serialize(requestData, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://api.aimlapi.com/v1/chat/completions"),
                Headers =
                {
                    { "Authorization", "Bearer <944ef9b30cc545b69ec639c1d9091de9>" },
                },
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };

            HttpResponseMessage response = await Client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
        catch (Exception ex)
        {
            throw new ExternalException(ex.Message, ex);
        }
    }

    public async Task<bool> CreatequestionLog(QuestionLog questionLog)
    {
        var isAdded = await _questionLogRepository.Create(questionLog);
        return isAdded;
    }
}