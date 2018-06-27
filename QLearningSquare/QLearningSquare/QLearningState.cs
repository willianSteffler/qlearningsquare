using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLearningSquare
{
    public class QLearningState
    {
        public List<string> Siblings;
        
        //ActionName,Reward
        public Dictionary<string, int> Actions;

        public int StateReward;
        public string Name;

    }
}
