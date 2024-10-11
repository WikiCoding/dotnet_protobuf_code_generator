namespace ClassToProtoMapper.Services;

internal static class TypeConverterPrimitives
{
    public static string Convert(string type) => type switch
    {
        "Int32" => "int32",
        "Double" => "double",
        "Single" => "float",
        "Int64" => "int64",
        "String" => "string",
        "Boolean" => "bool",
        "Int32[]" => "repeated int32",
        "Double[]" => "repeated double",
        "Single[]" => "repeated float",
        "Long[]" => "repeated long",
        "Boolean[]" => "repeated bool",
        "String[]" => "repeated string",
        // need to add an import statement => import "google/protobuf/timestamp.proto";
        "DateTime" => "google.protobuf.Timestamp",
        "DateOnly" => "google.protobuf.Timestamp",
        _ => throw new InvalidDataException("Couldn't convert data type"),
    };
}
