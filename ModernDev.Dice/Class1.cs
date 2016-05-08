using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace ModernDev.Dice
{
    public class Dice
    {
        public Dice()
        {
            
        }

        public bool Bool(int likelihood = 50)
        {
            if (likelihood < 0 || likelihood > 100)
            {
                throw new ArgumentOutOfRangeException(nameof(likelihood), "Likelihood accepts values from 0 to 100.");
            }

            return false;
        }
    }
}
