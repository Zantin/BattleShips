using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseNetwork
{
    public class NetworkObject
    {
        public int dataLenght { get { return objectLength + 4; } }
        public int objectLength {  get { return obj.Length; } }
        private byte[] obj;

        public NetworkObject(byte[] obj)
        {
            this.obj = obj;
        }

        /// <summary>
        /// Gets the length and the data inside the NetworkObject as a byte array.
        /// </summary>
        /// <returns>Byte[] where the first 4 bytes is the length of the data</returns>
        public byte[] GetData()
        {
            List<byte> data = new List<byte>();

            data.AddRange(GetDataLength(obj.Length));
            data.AddRange(obj);

            return data.ToArray();
        }

        /// <summary>
        /// Converts the integer provided to a byte array with the same value.
        /// </summary>
        /// <param name="dataLength"></param>
        /// <returns></returns>
        private byte[] GetDataLength(int dataLength)
        {
            byte[] length = new byte[4];

            length[0] = (byte)(dataLength >> 24);
            length[1] = (byte)(dataLength >> 16);
            length[2] = (byte)(dataLength >> 8);
            length[3] = (byte)(dataLength >> 0);

            return length;
        }
    }

}
