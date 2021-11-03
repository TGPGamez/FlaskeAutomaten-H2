using FlaskeAutomaten_H2.Lib.Automat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FlaskeAutomaten_H2.Lib.Machine
{
    public class Consumer
    {
        private static Random rand = new Random();

        public MessageEvent ConsumerInfo { get; set; }

        public BufferTray<Drink> Tray { get; private set; }
        public DrinkType TrayType { get; private set; }

        public Consumer(BufferTray<Drink> tray, DrinkType drinkType, MessageEvent consumerInfo)
        {
            Tray = tray;
            TrayType = drinkType;
            ConsumerInfo = consumerInfo;
        }

        /// <summary>
        /// Takes drink away from tray
        /// </summary>
        public void Take()
        {
            while (true)
            {
                lock (Tray)
                {
                    //Stop and wait until Tray has drink/drinks
                    while (Tray.Position == 0)
                    {
                        Monitor.PulseAll(Tray);
                        ConsumerInfo?.Invoke($"[{TrayType}Tray] Waiting for {TrayType}..");
                        Monitor.Wait(Tray);
                    }
                    //Pull drink from tray and log removed drink
                    Drink drink = Tray.Pull();
                    ConsumerInfo?.Invoke($"[{TrayType}Tray] Removed x1 {drink.Type} from tray");
                    Thread.Sleep(rand.Next(250, 450));
                }
            }
        }
    }
}
