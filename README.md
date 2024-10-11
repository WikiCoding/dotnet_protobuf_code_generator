# Summary
.NET Console app to automatically generate protofiles based on provided classes.
No support for different types of numbers (uint32, sint32, fixed...) as this may change within the same class, no support for option nor grpc services.
Maps can only contain primitive types as per Protobufs rules.
Serializing a protobuf class to Json, keeping descriptor names will, by protobufs design, translate long(int64) types to string. Caution with this! 
Also, as a note, when we compile the protofile (for csharp), the generated code will make all the attribute names in PascalCase even if they are in another type.

# Usage
1. It starts generating the headers. All the params are optional. We may want to incluide import for Timestamp (set to false by default), option or packageName. The Generate method returns a **StringBuilder** that must be used to apend the rest of the generated code.
```csharp
StringBuilder sb = ProtoFileHeadersGenerator.Generate(containsDateTimeImport: true, optionValue: "UnaryServer", packageName: "unary");
```
2. The headers as described in the previous line would look like this:
```proto
syntax="proto3";

option csharp_namespace = "UnaryServer";

package unary;

import "google/protobuf/timestamp.proto";
```

2. Add the classes you want to generate code for in the Models directory (or anywhere else you'd like). See example class:
```csharp
internal class ParentClass
{
    public ChildClass? NullableArrClass { get; set; } = null;
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

public class ChildClass
{
    public int child_member { get; set; }
}
```

3. Start generating the code using the **ClassToProtoMessageMapper<T>.Map(string intendedClassName)** static class. Since the provided class has properties which as also by itself classes, for a well formed protofile you must generate the code for the child classes and enums and save for last the parent class. The code returns a string to be appended to the string builder.
```csharp
string childClass = ClassToProtoMessageMapper<ChildClass>.Map(nameof(ChildClass));
sb.Append(childClass);

string statusEnum = ClassToProtoMessageMapper<Status>.Map(nameof(Status));
sb.Append(statusEnum);

...
```

4. When you're done with all the child classes, add the ParentClass. In this case, to make it clearer that the class is a Proto parent class you can append Proto to it's name
```csharp
string parentClass = ClassToProtoMessageMapper<ParentClass>.Map(nameof(ParentClass) + "Proto");
sb.Append(parentClass);
```

5. Now it's time to generate the protofile calling the **FileGenerator.Generate(string fileContent, string protoFileName, string? fileExtension = ".proto", string? path = null)**. By default the file will be generated in the **/proto** directory, but if you really want a different location you can specify the full path. The fileContent and protoFileName are mandatory
```csharp
FileGenerator.Generate(sb.ToString(), nameof(ParentClass) + "Proto");
```

6. Copy your proto file to where you intend to use it and run:
```bash
protoc --csharp_out=. <ProtoFileName.proto>
```