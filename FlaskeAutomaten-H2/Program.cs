using System;
using FlaskeAutomaten_H2.Lib;
using FlaskeAutomaten_H2.Lib.Automat;
using FlaskeAutomaten_H2.Lib.Machine;

namespace FlaskeAutomaten_H2
{
    class Program
    {
        static void Main(string[] args)
        {
            Manager manager = new Manager();
            manager.ProducerEventInfo += ManagerInfoEvent;
            manager.Start();
        }

        private static void ManagerInfoEvent(string message)
        {
            Console.WriteLine(message);
        }
    }
}
