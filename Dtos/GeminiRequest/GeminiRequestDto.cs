namespace GeminiAI.Dtos.GeminiRequest;
public class Content
{
    public List<Part> parts { get; set; }
}

public class Part
{
    public string text { get; set; }
}

public class GeminiRequestDto
{
    public List<Content> contents { get; set; }
}





