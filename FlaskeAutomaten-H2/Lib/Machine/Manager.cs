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
        public MessageEvent SorterEventInfo { get; set; }
        public MessageEvent ConsumerEventInfo { get; set; }

        public Manager()
        {
            MainTray = new BufferTray<Drink>(MAX_TRAY_ITEMS);
            SodaTray = new BufferTray<Drink>(MAX_TRAY_ITEMS);
            BeerTray = new BufferTray<Drink>(MAX_TRAY_ITEMS);
        }

        /// <summary>
        /// Makes all threads and starts them
        /// </summary>
        public void Start()
        {
            ThreadStart sodaThreadStarter = delegate { ConsumerProcess(SodaTray, DrinkType.Soda); };
            ThreadStart beerThreadStarter = delegate { ConsumerProcess(BeerTray, DrinkType.Beer); };
            
            Thread producerThread = new Thread(ProduceDrinkProcess);
            Thread sorterThread = new Thread(SorterProcess);
            Thread sodaConsumerThread = new Thread(sodaThreadStarter);
            Thread beerConsumerThread = new Thread(beerThreadStarter);
            
            producerThread.Start();
            sorterThread.Start();
            sodaConsumerThread.Start();
            beerConsumerThread.Start();
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

        private void SorterProcess()
        {
            DrinkSorter drinkSorter = new DrinkSorter(MainTray, SodaTray, BeerTray, SorterEventInfo);
            try
            {
                drinkSorter.Sort();
            }
            catch (Exception ex)
            {
                SorterEventInfo?.Invoke(ex.Message);
            }
        }

        private void ConsumerProcess(BufferTray<Drink> tray, DrinkType type)
        {
            Consumer consumer = new Consumer(tray, type, ConsumerEventInfo);
            try
            {
                consumer.Take();
            }
            catch (Exception ex)
            {
                ConsumerEventInfo?.Invoke(ex.Message);
            }
        }

        private void BeerConsumerProcess()
        {
            Consumer beerConsumer = new Consumer(BeerTray, DrinkType.Beer, ConsumerEventInfo);
            try
            {
                beerConsumer.Take();
            }
            catch (Exception ex)
            {
                ConsumerEventInfo?.Invoke(ex.Message);
            }
        }
    }
}
