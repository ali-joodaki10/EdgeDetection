using Application.Mappers;
using Application.Models;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services;

public sealed class EdgeDetectionService
{
    private readonly IEdgeDetectionService _edgeService;
    private readonly ImageMapper _imageMapper;

    public EdgeDetectionService(IEdgeDetectionService edgeService, ImageMapper imageMapper)
    {
        _edgeService = edgeService;
        _imageMapper = imageMapper;
    }

    public string Process(ImageRequest image)
    {
        var mapped = _imageMapper.Map(image);
        return _edgeService.DetectEdges(mapped);
    }
}
