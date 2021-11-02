using FlaskeAutomaten_H2.Lib.Automat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FlaskeAutomaten_H2.Lib.Machine
{
    public class DrinkSorter
    {
        private static Random rand = new Random();

        public BufferTray<Drink> Maintray { get; private set; }
        public BufferTray<Drink> SodaTray { get; private set; }
        public BufferTray<Drink> BeerTray { get; private set; }

        public void Sort()
        {
            while (true)
            {
                lock (Maintray)
                {
                    while (Maintray.Length == 0)
                    {
                        Monitor.Wait(Maintray);
                    }
                    Drink drink = Maintray.Pull();
                    BufferTray<Drink> tray = GetTrayFromDrinkType(drink.Type);
                    if (tray.Position < tray.Length)
                    {
                        lock (tray)
                        {
                            tray.Push(drink);
                            //Log event

                            Thread.Sleep(rand.Next(50, 250));
                        }
                    } 
                    else
                    {
                        Maintray.Push(drink);
                    }
                }
            }
        }


        private BufferTray<Drink> GetTrayFromDrinkType(DrinkType drinkType)
        {
            switch (drinkType)
            {
                case DrinkType.Soda:
                    return SodaTray;
                case DrinkType.Beer:
                    return BeerTray;
            }
            return null;
        }

    }
}
