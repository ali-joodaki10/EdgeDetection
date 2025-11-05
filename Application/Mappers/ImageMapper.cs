using Application.Models;
using Domain.Entities;

namespace Application.Mappers;

public sealed class ImageMapper
{
    public InputImage Map(ImageRequest request)
    {
        return new InputImage(request.FilePath);
    }
}