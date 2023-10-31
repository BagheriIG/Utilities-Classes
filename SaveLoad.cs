using Newtonsoft.Json;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Utilities
{
    public enum SaveType { Binary, Excel, XML, Json }

    public class SaveLoad
    {
        public static void SaveToFile<T>(T saveObject, string savePath, SaveType saveType = SaveType.Json)
        {
            switch (saveType)
            {
                case SaveType.Binary:
                    using (MemoryStream ms = new MemoryStream())
                    {
                        BinaryFormatter formatter = new BinaryFormatter();
                        formatter.Context = new StreamingContext(StreamingContextStates.Clone);
                        formatter.Serialize(ms, saveObject);
                        ms.Position = 0;
                        using (FileStream file = new FileStream(savePath, FileMode.Create, FileAccess.Write))
                            ms.CopyTo(file);
                    }
                    break;
                case SaveType.Json:
                    string jsonString = JsonConvert.SerializeObject(saveObject, setting: new JsonSerializerSettings() { Formatting = Formatting.None, ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
                    byte[] inputBytes = Encoding.UTF8.GetBytes(jsonString);
                    using (FileStream outputFileStream = new FileStream(savePath, FileMode.Create))
                    {
                        using (GZipStream compressionStream = new GZipStream(outputFileStream, CompressionMode.Compress))
                        {
                            compressionStream.Write(inputBytes, 0, inputBytes.Length);
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        public static T LoadFromFile<T>(string filePath, SaveType saveType = SaveType.Json)
        {
            switch (saveType)
            {
                case SaveType.Binary:
                    using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            file.CopyTo(ms);
                            BinaryFormatter formatter = new BinaryFormatter();
                            formatter.Context = new StreamingContext(StreamingContextStates.Clone);
                            ms.Position = 0;
                            return (T)formatter.Deserialize(ms);
                        }
                    }
                case SaveType.Json:
                    string jsonString = string.Empty;
                    using (FileStream inputFileStream = new FileStream(filePath, FileMode.Open))
                    {
                        using (GZipStream decompressionStream = new GZipStream(inputFileStream, CompressionMode.Decompress))
                        {
                            using (MemoryStream outputStream = new MemoryStream())
                            {
                                byte[] outputBytes = new byte[4096];
                                int bytesRead;
                                while ((bytesRead = decompressionStream.Read(outputBytes, 0, outputBytes.Length)) > 0)
                                {
                                    outputStream.Write(outputBytes, 0, bytesRead);
                                }

                                jsonString = Encoding.UTF8.GetString(outputStream.ToArray());
                            }
                        }
                    }
                    return JsonConvert.DeserializeObject<T>(jsonString);
                default:
                    return default;
            }
        }
    }
}