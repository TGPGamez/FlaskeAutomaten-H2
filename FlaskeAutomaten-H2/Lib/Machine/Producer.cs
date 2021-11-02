using FlaskeAutomaten_H2.Lib.Automat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FlaskeAutomaten_H2.Lib.Machine
{
    public class Producer
    {
        private static Random rand = new Random();
        public BufferTray<Drink> Maintray { get; private set; }
        public MessageEvent ProducerInfo { get; set; }

        public Producer(BufferTray<Drink> tray, MessageEvent producerInfo)
        {
            Maintray = tray;
            ProducerInfo = producerInfo;
        }
        
        public void MakeDrinks()
        {
            while (true)
            {
                lock (Maintray)
                {
                    while (Maintray.Position >= Maintray.Length)
                    {
                        ProducerInfo?.Invoke("[MainTray] Is full, waiting for space..");
                        Monitor.Wait(Maintray);
                    }
                    for (int i = Maintray.Position; i < Maintray.Length; i++)
                    {
                        DrinkType drinkType = (DrinkType)(rand.Next(0, 2));
                        Maintray.PushToFront(new Drink(drinkType));
                        ProducerInfo?.Invoke($"[MainTray] Produced drink: {drinkType} ({Maintray.Position}/{Maintray.Length})");
                        Thread.Sleep(rand.Next(200, 500));
                    }
                    Monitor.PulseAll(Maintray);
                }
            }
        }
    }
}
