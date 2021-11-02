using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlaskeAutomaten_H2.Lib.Automat
{
    public class Drink
    {
        public DrinkType Type { get; private set; }

        public Drink(DrinkType type)
        {
            Type = type;
        }
    }
}
