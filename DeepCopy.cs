using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Utilities
{
    public class DeepCopy
    {
        public static T Clone<T>(T other)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Context = new StreamingContext(StreamingContextStates.Clone);
                formatter.Serialize(ms, other);
                ms.Position = 0;
                return (T)formatter.Deserialize(ms);
            }
        }
    }
}
