using Domain.Entities;
using Infrastructure.ImageProcessing;

namespace Tests
{
    public class OpenCvEdgeDetectionServiceTests
    {
        [Fact]
        public void DetectEdges_ShouldReturnOutputPath_WhenImageIsValid()
        {
            // Arrange
            var service = new OpenCvEdgeDetectionService();

            var testImagePath = Path.Combine(Directory.GetCurrentDirectory(), "test1.jpg");

            var input = new InputImage(testImagePath);

            // Act
            var outputPath = service.DetectEdges(input);

            // Assert
            Assert.True(File.Exists(outputPath), "Output file was not created.");
        }
    }
}
