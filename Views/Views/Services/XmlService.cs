using System.IO;
using System.Xml.Serialization;

namespace RGZ_TIMP.Services
{
    public static class XmlService
    {
        public static void SaveArray(
            int[] array,
            string path)
        {
            XmlSerializer serializer =
                new(typeof(int[]));

            using FileStream stream =
                new(path, FileMode.Create);

            serializer.Serialize(stream, array);
        }

        public static int[] LoadArray(string path)
        {
            XmlSerializer serializer =
                new(typeof(int[]));

            using FileStream stream =
                new(path, FileMode.Open);

            return (int[])serializer.Deserialize(stream)!;
        }
    }
}