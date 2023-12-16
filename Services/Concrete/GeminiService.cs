using GeminiAI.Common;
using GeminiAI.Dtos.GeminiRequest;
using GeminiAI.Dtos.GeminiResponse;
using GeminiAI.Services.Abstract;
using Newtonsoft.Json;

namespace GeminiAI.Services.Concrete;

public class GeminiService : IGeminiService
{
    private readonly AppSettings _appSettings;
    private readonly HttpClient _httpTextOnlyClient;
    private readonly HttpClient _httpTextAndImageClient;
    public GeminiService(AppSettings appSettings, IHttpClientFactory httpClientFactory)
    {
        _appSettings = appSettings;
        _httpTextOnlyClient = httpClientFactory.CreateClient("GeminiAITextOnly");
        _httpTextAndImageClient = httpClientFactory.CreateClient("GeminiAITextAndImage");
    }

    public async Task<GeminiResponseDto> GetTextAndImage(Stream file, string prompt, CancellationToken cancellationToken = default)
    {
        byte[] data;
        await using (var memoryStream = new MemoryStream())
        {
            await file.CopyToAsync(memoryStream);
            data = memoryStream.ToArray();
        }
        var response = await _httpTextAndImageClient.PostAsJsonAsync($"?key={_appSettings.API_KEY}", GetTextAndImageRequest(data, prompt), cancellationToken);
        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            var geminiResponse = JsonConvert.DeserializeObject<GeminiResponseDto>(responseContent);
            return geminiResponse;
        }
        return null;
    }

    public async Task<GeminiResponseDto> GetTextOnly(string prompt, CancellationToken cancellationToken)
    {
        var response = await _httpTextOnlyClient.PostAsJsonAsync($"?key={_appSettings.API_KEY}", GetTextOnlyRequest(prompt), cancellationToken);
        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            var geminiResponse = JsonConvert.DeserializeObject<GeminiResponseDto>(responseContent);
            return geminiResponse;
        }
        return null;


    }


    private GeminiRequestDto GetTextAndImageRequest(byte[] data, string prompt)
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
                        },
                        new Dtos.GeminiRequest.Part
                        {
                            inline_data = new InlineData
                                {
                                    mime_type = "image/jpeg",
                                    data = data
                                }
                        }
                    }
                }
            }
        };
        return request;
    }


    private GeminiRequestDto GetTextOnlyRequest(string prompt)
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