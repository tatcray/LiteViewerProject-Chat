using AnySerializer;
using BinaryPack;
using Snitch.Core.Abstractions;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Snitch.Core.Utilities
{
    public static class DataConverter
    {
        /*
        public static byte[] ObjectToByteArray(object obj)
        {
            if (obj == null)
                return (byte[])null;
            return Serializer.Serialize(obj);
        }

        public static T ByteArrayToObject<T>(byte[] arrBytes)
        {
            return Serializer.Deserialize<T>(arrBytes, SerializerOptions.None);
        }
        */
        /*
        public static byte[] ObjectToByteArray(object obj)
        {
            if (obj == null)
                return (byte[])null;
            return BinaryConverter.Serialize(obj);
        }
        */
        /*
        public static T ByteArrayToObject<T>(byte[] arrBytes)
        {
            return BinaryConverter.Deserialize<T>(arrBytes);
        }
        */


        public static byte[] ObjectToByteArray(object obj)
        {
            if (obj == null)
                return null;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                new BinaryFormatter().Serialize(memoryStream, obj);
                return memoryStream.ToArray();
            }
        }

        public static T ByteArrayToObject<T>(byte[] arrBytes)
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            T obj;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                memoryStream.Write(arrBytes, 0, arrBytes.Length);
                memoryStream.Seek(0L, SeekOrigin.Begin);
                obj = (T)binaryFormatter.Deserialize(memoryStream);
            }
            return obj;
        }

    }
}
