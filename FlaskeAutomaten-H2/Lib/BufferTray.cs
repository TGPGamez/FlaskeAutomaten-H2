using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlaskeAutomaten_H2.Lib
{
    public class BufferTray<T>
    {
        public int Length { get; private set; }
        public T[] Buffer { get; set; }
        public int Position { get; set; } = 0;

        public BufferTray(int length)
        {
            Length = length;
            Buffer = new T[length];
        }

        public void Push(T type)
        {
            Buffer[Position] = type;
            Position++;
        }

        public T Pull()
        {
            Position--;
            return Buffer[Position];
        }
    }
}
