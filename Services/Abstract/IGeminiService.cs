using GeminiAI.Dtos.GeminiResponse;

namespace GeminiAI.Services.Abstract
{
    public interface IGeminiService
    {
        Task<GeminiResponseDto> GetContent(string prompt , CancellationToken cancellationToken = default);
    }
}
