using FlaskeAutomaten_H2.Lib.Automat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FlaskeAutomaten_H2.Lib.Machine
{
    public class Manager
    {
        private const int MAX_TRAY_ITEMS = 10;

        public BufferTray<Drink> MainTray;
        public BufferTray<Drink> SodaTray;
        public BufferTray<Drink> BeerTray;

        public MessageEvent ProducerEventInfo { get; set; }

        public Manager()
        {
            MainTray = new BufferTray<Drink>(MAX_TRAY_ITEMS);
            SodaTray = new BufferTray<Drink>(MAX_TRAY_ITEMS);
            BeerTray = new BufferTray<Drink>(MAX_TRAY_ITEMS);
        }

        public void Start()
        {
            Thread producerThread = new Thread(ProduceDrinkProcess);
            producerThread.Start();
        }

        private void ProduceDrinkProcess()
        {
            Producer producer = new Producer(MainTray, ProducerEventInfo);
            try
            {
                producer.MakeDrinks();
            }
            catch (Exception ex)
            {
                ProducerEventInfo?.Invoke(ex.Message);
            }
        }
    }
}
