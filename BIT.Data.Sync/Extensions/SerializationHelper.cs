using System;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace BIT.Data.Sync.Extensions
{
    public static class SerializationHelper
    {
        public static byte[] CompressCore(byte[] data)
        {
            using (var compressedStream = new MemoryStream())
            using (var zipStream = new GZipStream(compressedStream, CompressionMode.Compress))
            {
                zipStream.Write(data, 0, data.Length);
                zipStream.Close();
                return compressedStream.ToArray();
            }
        }
        public static byte[] DecompressCore(byte[] data)
        {
            using (var compressedStream = new MemoryStream(data))
            using (var zipStream = new GZipStream(compressedStream, CompressionMode.Decompress))
            using (var resultStream = new MemoryStream())
            {
                zipStream.CopyTo(resultStream);
                return resultStream.ToArray();
            }
        }

        public static byte[] SerializeCore(object Instance)
        {
            string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(Instance);

            byte[] bytes = Encoding.UTF8.GetBytes(jsonString);
            return bytes;
           
        }
        public static T DeserializeCore<T>(byte[] Instance)
        {
            string str = Encoding.UTF8.GetString(Instance);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(str);
           
        }

        public static IDelta CreateDeltaCore(string Identity, object Operations)
        {
            DateTime now = DateTime.Now;
            var delta = new Delta() { Date = now, Identity = Identity,  Operation = CompressCore(SerializeCore(Operations)) };
            delta.Epoch = now.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
            return delta;
        }
    }
}
