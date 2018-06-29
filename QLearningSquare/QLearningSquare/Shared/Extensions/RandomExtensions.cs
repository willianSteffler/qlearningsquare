using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLearningSquare.Shared.Extensions
{
    public static class RandomExtensions
    {
        private static readonly Random getrandom = new Random();

        public static int GetPseudoRandom(int min, int max)
        {
            lock (getrandom)
            {
                return getrandom.Next(min, max);
            }
        }
    }
}
