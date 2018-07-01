using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLearningSquare.Shared.Extensions;
using QLearningSquare.AppMediator;

namespace QLearningSquare
{
    public class QLearningController : IDisposable
    {
        public Dictionary<string, QLearningState> States = new Dictionary<string, QLearningState>();
        QLearningWorker worker;

        double randomCount;

        public Action OnGoalReached;
        public int AleatoryPercentage = 45;
        public double LearningFactor = 0.5;

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
            bool useRnd = (worker.Steps != 0) && worker.CurrentState.ActionsAreEqual() && ((randomCount / worker.Steps) * 100) < AleatoryPercentage;
            QLearningAction takenAction = worker.CurrentState.Max(useRnd);
            QLearningState nextState = States[takenAction.StateResult];
            QLearningAction maxActionNextState = nextState.Max(false);

            takenAction.Reward = worker.CurrentState.StateReward + (LearningFactor * maxActionNextState.Reward);
            worker.CurrentState = nextState;
            worker.Steps++;
            if (useRnd)
                randomCount++;

            if (worker.CurrentState.Type == StateType.GoalState)
            {
                
                OnGoalReached?.Invoke();

                worker.Steps = 0;
                randomCount = 0;
                worker.CurrentState = GetState(Mediator.pMediator.getInitialStateName());
            }

        }
        

        public void Dispose()
        {
            States.Clear();
            worker = null;
        }
    }
}
