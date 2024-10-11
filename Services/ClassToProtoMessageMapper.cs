using ClassToProtoMapper.Models;
using System.Reflection;
using System.Text;

namespace ClassToProtoMapper.Services;

internal static class ClassToProtoMessageMapper<T> 
{

    public static string Map(string className)
    {
        var sb = new StringBuilder();

        PropertyInfo[] entityProps = typeof(T).GetProperties();

        if (!typeof(T).IsEnum && !typeof(T).IsClass) { throw new DataMisalignedException("The data types acceped are class and enum"); }

        if (typeof(T).IsEnum)
        {
            sb.Append($"enum {className} " + "{\n");
            GenerateEnumProto(sb);
        }

        if (typeof(T).IsClass)
        {
            sb.Append($"message {className} " + "{\n");
            GenerateMessageAttributes(sb, entityProps);
        }

        sb.AppendLine("}");

        return sb.ToString();
    }

    private static int GenerateEnumProto(StringBuilder sb)
    {
        int currIndex = 0;
        var enumValues = typeof(T).GetFields();

        for (int i = 1; i < enumValues.Length; i++)
        {
            sb.Append($"\t{enumValues[i].Name} = {currIndex};" + "\n");
            currIndex++;
        }

        return currIndex;
    }

    private static int GenerateMessageAttributes(StringBuilder sb, PropertyInfo[] entityProps)
    {
        int currIndex = 1;

        foreach (PropertyInfo entityProp in entityProps)
        {
            bool processed = false;

            if (entityProp.PropertyType.IsGenericType)
            {
                if (entityProp.PropertyType.Name.Equals("Nullable`1"))
                {
                    string primitiveType = TypeConverterPrimitives.Convert(entityProp.PropertyType.UnderlyingSystemType.GenericTypeArguments[0].Name);
                    sb.Append($"\t{primitiveType} {entityProp.Name} = {currIndex};" + "\n");
                } else {
                    AppendCollectionType(currIndex, sb, entityProp);
                }
                processed = true;
            }

            if (entityProp.PropertyType.IsPrimitive
                || entityProp.PropertyType.Name.Equals("String")
                || entityProp.PropertyType.Name.Equals("DateTime")
                || entityProp.PropertyType.Name.Equals("DateOnly")
                || entityProp.PropertyType.IsArray)
            {
                AppendPrimitiveType(currIndex, sb, entityProp);
                processed = true;
            }

            if (entityProp.PropertyType.IsEnum)
            {
                sb.Append($"\t{entityProp.PropertyType.Name} {entityProp.Name} = {currIndex};\n");
                processed = true;
            }

            if (!processed)
            {
                sb.Append($"\t{entityProp.PropertyType.Name} {entityProp.Name} = {currIndex};\n");
            }

            currIndex++;
        }

        return currIndex;
    }

    private static void AppendPrimitiveType(int currIndex, StringBuilder sb, PropertyInfo entityProp)
    {
        string primitiveType = TypeConverterPrimitives.Convert(entityProp.PropertyType.UnderlyingSystemType.Name);
        sb.Append($"\t{primitiveType} {entityProp.Name} = {currIndex};" + "\n");
    }

    private static void AppendCollectionType(int currIndex, StringBuilder sb, PropertyInfo entityProp)
    {
        Type collectionType = entityProp.PropertyType.GetGenericTypeDefinition();

        if (collectionType == typeof(List<>) || collectionType == typeof(HashSet<>))
        {
            string listType = entityProp.PropertyType.UnderlyingSystemType.GenericTypeArguments[0].Name;

            // check if it's primitive, otherwise append Proto to the name
            if (entityProp.PropertyType.UnderlyingSystemType.GenericTypeArguments[0].IsPrimitive)
            {
                sb.Append($"\trepeated {listType} {entityProp.Name} = {currIndex};" + "\n");
            }
            else
            {
                sb.Append($"\trepeated {listType} {entityProp.Name} = {currIndex};" + "\n");
            }
        }

        if (collectionType == typeof(Dictionary<,>))
        {
            Type[] genericTypeArgs = entityProp.PropertyType.UnderlyingSystemType.GenericTypeArguments;

            string keyType = genericTypeArgs[0].Name;
            string valueType = genericTypeArgs[1].Name;

            string key = TypeConverterPrimitives.Convert(keyType);
            string val = TypeConverterPrimitives.Convert(valueType);

            sb.Append($"\tmap<{key}, {val}> {entityProp.Name} = {currIndex};" + "\n");
        }
    }
}
