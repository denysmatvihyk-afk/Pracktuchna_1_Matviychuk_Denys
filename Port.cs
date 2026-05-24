using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Praktychna1
{
    public class Port : ICloneable
    {
        public int PortNumber { get; }

        public byte[] DataBuffer { get; } = new byte[64]; // Одновимірний масив
        public bool IsOpen { get; private set; }
        public string DeviceName { get; }
        public Port(int number, string deviceName)
        {
            PortNumber = number;
            DeviceName = deviceName;
        }
        public void Open() => IsOpen = true;
        public void Close() => IsOpen = false;
        public object Clone()
        {
            var clone = (Port)this.MemberwiseClone();
            Array.Copy(this.DataBuffer, clone.DataBuffer, 64);
            return clone;
        }
    }
}
