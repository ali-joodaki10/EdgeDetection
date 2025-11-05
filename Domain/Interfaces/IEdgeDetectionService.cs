using Domain.Entities;

namespace Domain.Interfaces;

public interface IEdgeDetectionService
{
    string DetectEdges(InputImage image);
}
