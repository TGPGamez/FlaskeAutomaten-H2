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
        public MessageEvent DrinkSorterInfo { get; set; }

        public BufferTray<Drink> Maintray { get; private set; }
        public BufferTray<Drink> SodaTray { get; private set; }
        public BufferTray<Drink> BeerTray { get; private set; }

        public DrinkSorter(BufferTray<Drink> mainTray, BufferTray<Drink> sodaTray, BufferTray<Drink> beerTray, MessageEvent sorterInfo)
        {
            Maintray = mainTray;
            SodaTray = sodaTray;
            BeerTray = beerTray;
            DrinkSorterInfo = sorterInfo;
        }

        /// <summary>
        /// Sorts the different drinks out to the right tray from the Maintray
        /// </summary>
        public void Sort()
        {
            while (true)
            {
                lock (Maintray)
                {
                    //Stop sorting if Maintray is empty
                    while (Maintray.Position == 0)
                    {
                        Monitor.PulseAll(Maintray);
                        DrinkSorterInfo?.Invoke("[Sorter] Waiting for drinks..");
                        Monitor.Wait(Maintray);
                    }
                    //Pull drink from Maintray
                    Drink drink = Maintray.Pull();
                    //Get the right tray
                    BufferTray<Drink> tray = GetTrayFromDrinkType(drink.Type);
                    if (tray.Position < tray.Length)
                    {
                        lock (tray)
                        {
                            //Push drink to right tray
                            tray.Push(drink);
                            //Log sorted drink to tray
                            DrinkSorterInfo?.Invoke($"[Sorter] Sorted {drink.Type} to {drink.Type}Tray ({tray.Position}/{tray.Length})");
                            Monitor.PulseAll(tray);

                            Thread.Sleep(rand.Next(250, 450));
                        }
                        if (tray.Position == tray.Length)
                        {
                            DrinkSorterInfo?.Invoke($"[Sorter] {drink.Type}Tray is full..");
                        }
                    } 
                    else
                    {
                        Maintray.Push(drink);
                    }
                }
            }
        }

        /// <summary>
        /// Returns Tray out from drinkType
        /// </summary>
        /// <param name="drinkType"></param>
        /// <returns></returns>
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
