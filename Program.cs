// See https://aka.ms/new-console-template for more information
using System.Reflection;
using ImageMagick.Drawing;
using ImageMagick;

namespace MagickNetTest;

public class Program
{
    public static void Main(string[] args)
    {
        var assembly = Assembly.GetExecutingAssembly();

        var resourceNames = assembly.GetManifestResourceNames();
        string font1 = LoadFontFromResourceForMagickNet(assembly, $"{typeof(Program).Namespace}.Resources.font1.ttf");
        string font2 = LoadFontFromResourceForMagickNet(assembly, $"{typeof(Program).Namespace}.Resources.font2.otf");

        if (!File.Exists(font1) || !File.Exists(font2))
        {
            throw new FileNotFoundException("Font file not found.");
        }

        using var image = new MagickImage(MagickColors.White, 1000, 800);
        // Uncomment to make font1 work. Font2 will never show.
        //image.Settings.Font = font1;
        //image.Settings.FontFamily = font1;

        var drawables = new Drawables();

        drawables.FontPointSize(40)
                    .Font(font1)
                    .FillColor(MagickColors.Black)
                    .TextAlignment(TextAlignment.Left)
                    .Text(100, 50, "This is such a nice font!");

        drawables.FontPointSize(40)
                    .Font(font2)
                    .FillColor(MagickColors.Black)
                    .TextAlignment(TextAlignment.Left)
                    .Text(100, 100, "This is even better, or does it look the same?");

        drawables.Draw(image);
        var result = image.ToByteArray(MagickFormat.Png);
        File.WriteAllBytes($"image.png", result);
    }

    static string LoadFontFromResourceForMagickNet(Assembly assembly, string resourceName)
    {
        using Stream? fontStream = assembly.GetManifestResourceStream(resourceName);
        if (fontStream == null)
        {
            throw new FileNotFoundException($"Resource {resourceName} not found");
        }

        // Create a temporary file to store the font
        string tempFontPath = Path.GetTempFileName();
        using (var fileStream = new FileStream(tempFontPath, FileMode.Create, FileAccess.Write))
        {
            fontStream.CopyTo(fileStream);
        }

        return tempFontPath;
    }
}

