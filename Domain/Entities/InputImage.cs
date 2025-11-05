namespace Domain.Entities;

public sealed class InputImage
{
    public string FilePath { get; }
    public InputImage(string filePath)
    {
        FilePath = filePath;
    }
}
