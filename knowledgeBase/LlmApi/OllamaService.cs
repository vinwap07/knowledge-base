using System.Text;

namespace knowledgeBase;

public class OllamaService : IAiService
{
    private readonly string _apiKey;
    private readonly HttpClient _httpClient;
    private const string Prompt = @"Нужно написать саммари к тексту ниже. Ничего кроме саммари писать не нужно! 
        Саммари должно уменьшить объем текста вдвое как минимум. 
        Максимальный объем саммари - 3 абзаца.";

    public OllamaService(string apiKey, HttpClient httpClient)
    {
        _apiKey = apiKey;
        _httpClient = httpClient;
    }

    public async Task<string> SendRequest(string text)
    {
        if (!string.IsNullOrEmpty(_apiKey))
        {
            _httpClient.DefaultRequestHeaders.Authorization = 
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _apiKey);
        }

        var jsonData = $@"
        {{
            ""model"": ""gpt-oss:120b"",
            ""prompt"": ""{EscapeJsonString(Prompt + text)}"",
            ""stream"": false
        }}";

        var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
        
        var response = await _httpClient.PostAsync("https://ollama.com/api/generate", content);
        response.EnsureSuccessStatusCode();
        
        return await response.Content.ReadAsStringAsync();
    }
        
    private static string EscapeJsonString(string value)
    {
        return value.Replace("\\", "\\\\")
            .Replace("\"", "\\\"")
            .Replace("\n", "\\n")
            .Replace("\r", "\\r")
            .Replace("\t", "\\t");
    }
}