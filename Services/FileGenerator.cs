using System.Text;

namespace ClassToProtoMapper.Services;

internal static class FileGenerator
{
    public static void Generate(string fileContent, string protoFileName, string? fileExtension = ".proto", string? path = null)
    {
        string fullPath = Directory.GetCurrentDirectory().Split("bin")[0] + (path is null ? $"/proto/{protoFileName}{fileExtension}" : path + $"{protoFileName}{fileExtension}");

        try
        {
            using (FileStream fs = File.Create(fullPath))
            {
                byte[] writeBytes = Encoding.UTF8.GetBytes(fileContent);

                Console.WriteLine($"Writing to path: {fullPath}");
                fs.Write(writeBytes, 0, writeBytes.Length);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}
