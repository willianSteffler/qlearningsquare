using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLearningSquare.Shared.Extensions;
using QLearningSquare.AppMediator;

namespace QLearningSquare
{
    public class QLearningController
    {
        public Dictionary<string, QLearningState> States = new Dictionary<string, QLearningState>();
        QLearningWorker worker;

        double randomCount;
        double totalCount;

        public Action OnGoalReached;
        public int AleatoryPercentage = 0;
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

        internal void SetWorker(QLearningWorker worker)
        {
            this.worker = worker;
        }

        public void DoWork()
        {
            bool useRnd = totalCount != 0 && ((randomCount / totalCount) * 100) < AleatoryPercentage;

            QLearningAction takenAction = Max(worker.CurrentState,useRnd);
            QLearningState nextState = States[takenAction.StateResult];
            QLearningAction maxActionNextState = Max(nextState,false);

            takenAction.Reward = worker.CurrentState.StateReward + (learningFactor * maxActionNextState.Reward);
            worker.CurrentState = nextState;
            worker.Steps++;

            if (worker.CurrentState.Type == StateType.GoalState)
            {
                OnGoalReached?.Invoke();
                worker.Steps = 0;
                totalCount = 0;
                randomCount = 0;
                worker.CurrentState = GetState(Mediator.pMediator.getInitialStateName());
            }

        }

        private QLearningAction Max(QLearningState qState, bool useRandom)
        {
            QLearningAction ret;
            if (qState.Type == StateType.GoalState)
                ret = new QLearningAction() { Reward = qState.StateReward };
            else
            {
                if (useRandom)
                {
                    ret = Enumerable.ToList(qState.Actions.Values)[RandomExtensions.GetPseudoRandom(0, qState.Actions.Values.Count)];
                    randomCount++;
                }
                else
                {
                    ret = (Enumerable.ToList(qState.Actions.Values).Max());
                }
            }

            totalCount++;
            return ret;
        }
    }
}
