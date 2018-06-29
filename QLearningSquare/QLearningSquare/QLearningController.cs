using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLearningSquare.Shared.Extensions;

namespace QLearningSquare
{
    public class QLearningController
    {
        public Dictionary<string, QLearningState> States = new Dictionary<string, QLearningState>();
        QLearningState workerState;

        int randomCount;
        public int TotalCount;

        public Action OnGoalReached;
        public int AleatoryPercentage = 30;
        public double learningFactor = 0.5;

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

        public void DoWork()
        {
            QLearningAction actionToTake = Max(workerState);
            QLearningState nextState = States[actionToTake.StateResult];
            QLearningAction maxActionNextState = Max(nextState);

            actionToTake.Reward = workerState.StateReward + (learningFactor * maxActionNextState.Reward);
            workerState = nextState;
        }

        private QLearningAction Max(QLearningState qState)
        {
            bool useRandom = TotalCount > 0 && (Math.Round((double)((randomCount / TotalCount) * 100)) < AleatoryPercentage);
            QLearningAction ret;

            if (useRandom)
            {
                ret = Enumerable.ToList(qState.Actions.Values)[RandomExtensions.GetPseudoRandom(0, qState.Actions.Values.Count)];
                randomCount++;
            }
            else
            {
                ret = (Enumerable.ToList(qState.Actions.Values).Max());
            }

            TotalCount++;
            return ret;
        }
    }
}
