using GeminiAI.Dtos.GeminiResponse;

namespace GeminiAI.Services.Abstract
{
    public interface IGeminiService
    {
        Task<GeminiResponseDto> GetTextOnly(string prompt, CancellationToken cancellationToken = default);
        Task<GeminiResponseDto> GetTextAndImage(Stream file ,string prompt, CancellationToken cancellationToken = default);
    }
}
