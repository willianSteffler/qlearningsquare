using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLearningSquare
{
    public class QLearningController
    {
        public Dictionary<string, QLearningState> States = new Dictionary<string, QLearningState>();
        QLearningState workerState;

        public void AddState(QLearningState state)
        {
            States.Add(state.Name, state);
        }

        public QLearningState GetState(string stateName)
        {
            if (States.ContainsKey(stateName))
                return States[stateName];
            else
                return null;
        }

        public int GetStatesCount()
        {
            return States.Count;
        }

        internal void setInitialState(string v)
        {
            workerState = GetState(v);
        }
    }
}
