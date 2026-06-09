using System.IO;
using System.Xml.Serialization;

namespace SortVisualizer.Services
{
    public class XmlService
    {
        public void Save(string path, int[] array)
        {
            XmlSerializer serializer =
                new(typeof(int[]));

            using FileStream stream =
                new(path, FileMode.Create);

            serializer.Serialize(stream, array);
        }

        public int[] Load(string path)
        {
            XmlSerializer serializer =
                new(typeof(int[]));

            using FileStream stream =
                new(path, FileMode.Open);

            return (int[])serializer.Deserialize(stream);
        }
    }
}