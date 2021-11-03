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

        /// <summary>
        /// Method used to generate/make random drinks
        /// After updating tray we log to console what was produced
        /// and finally a random sleep to delay
        /// </summary>
        public void MakeDrinks()
        {
            while (true)
            {
                lock (Maintray)
                {
                    //Stop and wait until Maintray isn't full
                    while (Maintray.Position >= Maintray.Length)
                    {
                        ProducerInfo?.Invoke("[MainTray] Is full, waiting for space..");
                        Monitor.Wait(Maintray);
                    }
                    //Generate drink and add to tray
                    DrinkType drinkType = (DrinkType)(rand.Next(0, 2));
                    Maintray.PushToFront(new Drink(drinkType));
                    ProducerInfo?.Invoke($"[MainTray] Produced drink: {drinkType} ({Maintray.Position}/{Maintray.Length})");
                    Thread.Sleep(rand.Next(200, 400));

                    Monitor.PulseAll(Maintray);
                }
            }
        }
    }
}
