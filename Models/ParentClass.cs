using ClassToProtoMapper.Models;

namespace dotnet_class_to_protobuf_file_generator.Models;

internal class ParentClass
{
    public ChildClass? NullableChildClass { get; set; } = null;
    public List<ChildClass> ListChildClass { get; set; } = [];
    public string? NullableStr { get; set; }
    public float FloatingPointValue { get; set; }
    public double DoubleingValue { get; set;}
    public long? NullableLong { get; set; }    
    public long NormalLong { get; set; }
    public bool? BoolIsNull { get; set; }    
    public bool BoolIsntNull { get; set; }    
    public Dictionary<string, int> Dictionary { get; set; } = [];
    public Status Status { get; set; }
    public DateTime DateTime { get; set; }
}
