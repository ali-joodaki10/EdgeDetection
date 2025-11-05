using Application.Mappers;
using Application.Models;
using Application.Services;
using Domain.Interfaces;
using Infrastructure.ImageProcessing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddSingleton<IEdgeDetectionService, OpenCvEdgeDetectionService>();
        services.AddSingleton<ImageMapper>();
        services.AddSingleton<EdgeDetectionService>();
    })
    .Build();

var handler = host.Services.GetRequiredService<EdgeDetectionService>();

var inputImages = new[] { "Input 1.jpg", "Input 2.png" };

foreach (var fileName in inputImages)
{
    var filePath = Path.Combine(Directory.GetCurrentDirectory(), fileName);

    if (!File.Exists(filePath))
    {
        Console.WriteLine($"File not found: {filePath}");
        continue;
    }

    var image = new ImageRequest(filePath);
    var outputPath = handler.Process(image);
    Console.WriteLine($"Processed {filePath}");
    Console.WriteLine($"Output saved at: {outputPath}");
}