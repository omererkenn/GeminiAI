using GeminiAI.Common;
using GeminiAI.Dtos.GeminiRequest;
using GeminiAI.Dtos.GeminiResponse;
using GeminiAI.Services.Abstract;
using Newtonsoft.Json;

namespace GeminiAI.Services.Concrete;

public class GeminiService : IGeminiService
{
    private readonly AppSettings _appSettings;
    private readonly HttpClient _httpClient;
    public GeminiService(AppSettings appSettings, IHttpClientFactory httpClientFactory)
    {
        _appSettings = appSettings;
        _httpClient = httpClientFactory.CreateClient("GeminiAI");
    }

    public async Task<GeminiResponseDto> GetContent(string prompt, CancellationToken cancellationToken)
    {
        var response = await _httpClient.PostAsJsonAsync($"?key={_appSettings.API_KEY}", GetRequest(prompt), cancellationToken);
        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            var geminiResponse = JsonConvert.DeserializeObject<GeminiResponseDto>(responseContent);
            return geminiResponse;
        }
        return null;


    }


    private GeminiRequestDto GetRequest(string prompt)
    {
        var request = new GeminiRequestDto
        {
            contents = new List<Dtos.GeminiRequest.Content>
            {
                new Dtos.GeminiRequest.Content
                {
                    parts = new List<Dtos.GeminiRequest.Part>
                    {
                        new Dtos.GeminiRequest.Part
                        {
                            text = prompt,
                        }
                    }
                }
            }

        };
        return request;
    }
}