using System.Text;

namespace ClassToProtoMapper.Services;

internal class ProtoFileHeadersGenerator
{
    public static StringBuilder Generate(
        bool containsDateTimeImport = false,
        string? optionValue = null,
        string? packageName = null
        )
    {
        StringBuilder sb = new StringBuilder();
        AppendHeadersAndOptions(sb, optionValue, packageName);

        if (containsDateTimeImport)
        {
            sb.Append("import \"google/protobuf/timestamp.proto\";\n\n");
        }

        return sb;
    }

    private static void AppendHeadersAndOptions(StringBuilder sb, string? optionValue, string? packageName)
    {
        sb.Append($"syntax=\"proto3\";\n\n");

        if (optionValue is not null && !string.IsNullOrEmpty(optionValue))
        {
            sb.Append($"option csharp_namespace = \"{optionValue}\";\n\n");
        }

        if (packageName is not null && !string.IsNullOrEmpty(packageName))
        {
            sb.Append($"package {packageName};\n\n");
        }
    }
}
