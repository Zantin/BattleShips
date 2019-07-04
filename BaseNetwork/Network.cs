using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace BaseNetwork
{
    public abstract class Network
    {
        private static BinaryFormatter formatter = new BinaryFormatter();

        public static List<NetworkObject> GetAsNetworkObjects(params object[] objects)
        {
            List<NetworkObject> list = new List<NetworkObject>();
            using (MemoryStream ms = new MemoryStream())
            {
                foreach (object obj in objects)
                {
                    formatter.Serialize(ms, obj);
                    list.Add(new NetworkObject(ms.ToArray()));
                    ms.SetLength(0);
                }
            }
            return list;
        }

        /// <summary>
        /// Reads a byte from the NetworkStream and returns it as an integer.
        /// </summary>
        /// <param name="ns">The NetworkStream to read from</param>
        private static int GetAmountOfObjects(NetworkStream ns)
        {
            return ns.ReadByte();
        }

        /// <summary>
        /// Returns an object[] containing objects read from the NetworkStream.
        /// </summary>
        /// <param name="ns">The NetworkStream to read from</param>
        /// <param name="amountOfObjects">The amount of objects to return</param>
        private static object[] GetObjects(NetworkStream ns, int amountOfObjects)
        {
            object[] objs = new object[amountOfObjects];

            for (int i = 0; i < amountOfObjects; i++)
            {
                objs[i] = GetObject(ns);
            }
            return objs;
        }

        /// <summary>
        /// Returns an object from the NetworkStream (Using ReadObject and DeserializeObject).
        /// </summary>
        /// <param name="ns">The NetworkStream to read from</param>
        private static object GetObject(NetworkStream ns)
        {
            return DeserializeObject(ReadObject(ns));
        }

        /// <summary>
        /// Deserializes the byte[] and returns it as an object.
        /// </summary>
        private static object DeserializeObject(byte[] objectData)
        {
            object obj;

            using (MemoryStream ms = new MemoryStream(objectData))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                obj = formatter.Deserialize(ms);
            }

            return obj;
        }

        /// <summary>
        /// Reads an object from the NetworkStream and returns it as a byte-Array 
        /// </summary>
        /// <param name="ns">The NetworkStream to read from</param>
        private static byte[] ReadObject(NetworkStream ns)
        {
            int objectLength = GetObjectLength(ns);
            int dataRead = 0;
            byte[] objData = new byte[objectLength];

            do
            {
                dataRead += ns.Read(objData, dataRead, objectLength - dataRead);
            } while (dataRead < objectLength);

            return objData;
        }

        /// <summary>
        /// Reads the NetworkStream and returns everything in a packet.
        /// </summary>
        /// <param name="ns">The NetworkStream to read from</param>
        public static List<object> GetEverything(NetworkStream ns)
        {
            /* Creates a list of objects with 'n' objects 
             * List visual:
             * [byte amount of objects] (n)[object]
             */

            List<object> list = new List<object>();

            int amountOfObjects = GetAmountOfObjects(ns);
            list.Add(amountOfObjects);

            if (amountOfObjects > 0)
                list.AddRange(GetObjects(ns, amountOfObjects));

            return list;
        }

        /// <summary>
        /// Sends a packet using the provided NetworkStream containing the objects provided.
        /// Returns whether it failed or succeded.
        /// </summary>
        /// <param name="ns">The NetworkStream to send through</param>
        /// <param name="objects">The objects to send with the command (if any)</param>
        public static bool SendData(NetworkStream ns, params object[] objects)
        {
            try
            {
                List<NetworkObject> list = GetAsNetworkObjects(objects);
                ns.Write(new byte[] { (byte)list.Count }, 0, 1);
                foreach (NetworkObject no in list)
                {
                    ns.Write(no.GetData(), 0, no.dataLenght);
                }
                return true;
            }
            catch (Exception e)
            {
                //MessageBox.Show(e.StackTrace);
                return false;
            }
        }

        /// <summary>
        /// Returns the length of an object.
        /// </summary>
        /// <param name="ns">The NetworkStream to read from</param>
        private static int GetObjectLength(NetworkStream ns)
        {
            int length;

            length = (ns.ReadByte() << 24);
            length += (ns.ReadByte() << 16);
            length += (ns.ReadByte() << 8);
            length += (ns.ReadByte());

            return length;
        }
    }
}
