using ClassToProtoMapper.Models;
using ClassToProtoMapper.Services;
using dotnet_class_to_protobuf_file_generator.Models;
using System.Text;

namespace ClassToProtoMapper;

internal class Program
{
    static void Main(string[] args)
    {
        // building the headers
        StringBuilder sb = ProtoFileHeadersGenerator.Generate(containsDateTimeImport: true, optionValue: "UnaryServer", packageName: "unary");

        string childClass = ClassToProtoMessageMapper<ChildClass>.Map(nameof(ChildClass));
        sb.Append(childClass);

        string statusEnum = ClassToProtoMessageMapper<Status>.Map(nameof(Status));
        sb.Append(statusEnum);

        string parentClass = ClassToProtoMessageMapper<ParentClass>.Map(nameof(ParentClass) + "Proto");
        sb.Append(parentClass);

        FileGenerator.Generate(sb.ToString(), nameof(ParentClass) + "Proto");
    }
}
