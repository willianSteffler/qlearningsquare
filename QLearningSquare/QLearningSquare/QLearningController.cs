using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLearningSquare
{
    public class QLearningController
    {
        Dictionary<string, QLearningState> states = new Dictionary<string, QLearningState>();

        public void AddState(QLearningState state)
        {
            states.Add(state.Name, state);
        }

        public QLearningState GetState(string stateName)
        {
            if (states.ContainsKey(stateName))
                return states[stateName];
            else
                return null;
        }

        public int GetStatesCount()
        {
            return states.Count;
        }

    }
}
