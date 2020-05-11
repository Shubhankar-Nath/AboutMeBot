using System;
using System.Collections.Generic;
using System.Text;

namespace YodaBot.DataHandler.Processor
{
    public static class RandomSelector
    {
        public static int Rand(int max)
        {
            if (max > 0)
            {
                Random random = new Random();
                return random.Next(0, --max);
            }
            else
            {
                return 0;
            }

        }
    }
}
