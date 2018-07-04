using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLearningSquare
{
    public class QLearningEpisode : IComparable<QLearningEpisode>
    {
        public List<QLearningAction> takenActions = new List<QLearningAction>();
        public int TableAlterations;
        public int CompareTo(QLearningEpisode other)
        {
            if (takenActions.Count == other.takenActions.Count)
                return 0;
            else if (takenActions.Count > other.takenActions.Count)
                return 1;
            else
                return -1;
        }
    }
}
