using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlaskeAutomaten_H2.Lib
{
    public delegate void MessageEvent(string message);

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

        public void PushToFront(T type)
        {
            for (int i = Position; i > 0; i--)
            {
                Buffer[i] = Buffer[i - 1];
            }
            Buffer[0] = type;
            Position++;
        }

        public T Pull()
        {
            Position--;
            return Buffer[Position];
        }
    }
}
