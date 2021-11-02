using FlaskeAutomaten_H2.Lib.Automat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlaskeAutomaten_H2.Lib.Machine
{
    public class Consumer
    {
        public BufferTray<Drink> Tray { get; private set; }

        public Consumer(BufferTray<Drink> tray)
        {
            Tray = tray;
        }

        public void Take()
        {
            while (true)
            {

            }
        }
    }
}
