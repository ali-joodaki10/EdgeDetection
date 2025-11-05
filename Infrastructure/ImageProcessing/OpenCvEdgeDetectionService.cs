using Domain.Entities;
using Domain.Interfaces;
using OpenCvSharp;

namespace Infrastructure.ImageProcessing;

public sealed class OpenCvEdgeDetectionService : IEdgeDetectionService
{
    public string DetectEdges(InputImage image)
    {
        try
        {
            // 0. Load the source image in color mode
            using var src = Cv2.ImRead(image.FilePath, ImreadModes.Color);

            // 1. Convert to grayscale
            using var gray = new Mat();
            Cv2.CvtColor(src, gray, ColorConversionCodes.BGR2GRAY);

            // 2. Reduce noise using Gaussian blur
            using var blurred = new Mat();
            Cv2.GaussianBlur(gray, blurred, new Size(5, 5), 1.5);

            // 3. Apply binary thresholding (Otsu method)
            using var binary = new Mat();
            Cv2.Threshold(blurred, binary, 0, 255, ThresholdTypes.Binary | ThresholdTypes.Otsu);

            // 4. Find contours (only external)
            Cv2.FindContours(binary, out var contours, out _, RetrievalModes.External, ContourApproximationModes.ApproxNone);

            // 5. Filter out small contours: only keep large contours (area > 1000 pixels)
            const double minArea = 1000; // adjustable
            var largeContours = contours
                .Where(c => Cv2.ContourArea(c) > minArea)
                .ToList();

            // 6. If no large contours found, take the largest one
            if (!largeContours.Any() && contours.Any())
            {
                largeContours = contours
                    .OrderByDescending(c => Cv2.ContourArea(c))
                    .Take(1)
                    .ToList();
            }

            // 7. Create output image with white background
            using var output = new Mat(src.Size(), MatType.CV_8UC3, Scalar.White);

            // 8. Draw only large contours with blue color and thickness 3
            Cv2.DrawContours(output, largeContours, -1, new Scalar(255, 0, 0), thickness: 3);

            // 9. Save the output image
            var outputPath = Path.Combine(
                Path.GetDirectoryName(image.FilePath)!,
                "output_" + Path.GetFileName(image.FilePath));

            Cv2.ImWrite(outputPath, output);

            return outputPath;
        }
        catch (Exception ex)
        {
            // Log or rethrow the error
            Console.WriteLine($"Error processing image '{image.FilePath}': {ex.Message}");
            throw;
        }
    }

}
