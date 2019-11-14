using System.IO;
using Newtonsoft.Json;

public static class SerializeUtility
{
    public static string JsonNetSerialize<T>(this T obj) => JsonConvert.SerializeObject(obj);

    public static string JsonNetSerialize<T>(this T obj, Formatting formatting) => JsonConvert.SerializeObject(obj, formatting);

    public static string JsonNetSerialize<T>(this T obj, JsonSerializerSettings settings) => JsonConvert.SerializeObject(obj, settings);

    public static string JsonNetSerialize<T>(this T obj, Formatting formatting, JsonSerializerSettings settings) =>
        JsonConvert.SerializeObject(obj, formatting, settings);

    public static T JsonNetDeserialize<T>(this string serialized) => JsonConvert.DeserializeObject<T>(serialized);

    public static T JsonNetDeserializeFile<T>(this string filePath) => File.ReadAllText(filePath).JsonNetDeserialize<T>();

    public static void JsonNetSerializeFile<T>(this T obj, string filePath) => File.WriteAllText(filePath, obj.JsonNetSerialize());

    public static void JsonNetSerializeFile<T>(this T obj, string filePath, Formatting formatting) =>
        File.WriteAllText(filePath, obj.JsonNetSerialize(formatting));

    public static void JsonNetSerializeFile<T>(this T obj, string filePath, Formatting formatting, JsonSerializerSettings settings) =>
        File.WriteAllText(filePath, obj.JsonNetSerialize(formatting, settings));

    public static void JsonNetSerializeFile<T>(this T obj, string filePath, JsonSerializerSettings settings) =>
        File.WriteAllText(filePath, obj.JsonNetSerialize(settings));
}