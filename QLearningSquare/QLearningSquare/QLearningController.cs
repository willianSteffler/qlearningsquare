using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLearningSquare.Shared.Extensions;
using QLearningSquare.AppMediator;

namespace QLearningSquare
{
    public delegate void TerminalSolution(QLearningEpisode Solution);

    public class QLearningController : IDisposable
    {
        public Dictionary<string, QLearningState> States = new Dictionary<string, QLearningState>();
        QLearningWorker worker;

        double randomCount;
        

        LinkedList<QLearningEpisode> episodes = new LinkedList<QLearningEpisode>();
        QLearningEpisode currentEpisode = new QLearningEpisode();
        int maxActions;

        public int BestSolutionPatterCount = 10;
        public TerminalSolution ItsTerminal;
        public int AleatoryPercentage = 20;
        public double LearningFactor = 0.5;

        bool isworking;

        public void AddState(QLearningState state)
        {
            States.Add(state.Name, state);
            maxActions += state.Actions.Count;
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

        public void Init()
        {
            isworking = true;
        }

        public void DoWork()
        {
            if (!isworking)
                return;
            //worker.Steps != 0 && (randomCount/(double)workrer.Steps)*100 < AleatoryPercentage && worker.CurrentState.ActionsAreEqual() plusplus;
            bool useRnd = RandomExtensions.GetPseudoRandom(0,100) <= AleatoryPercentage;
            QLearningAction takenAction = worker.CurrentState.Max(useRnd);
            currentEpisode.takenActions.Add(takenAction);

            QLearningState nextState = States[takenAction.StateResult];
            QLearningAction maxActionNextState = nextState.Max(false);

            double val = worker.CurrentState.StateReward + (LearningFactor * maxActionNextState.Reward);
            if (val != takenAction.Reward)
            {
                takenAction.Reward = val;
                currentEpisode.TableAlterations++;
            }

            worker.CurrentState = nextState;
            worker.Steps++;
            
            if (useRnd)
                randomCount++;

            if (worker.CurrentState.Type == StateType.GoalState)
            {
                FinishEpisode();
            }

        }

        public void FinishEpisode()
        {

            if (episodes.Count >= BestSolutionPatterCount)
            {
                double avg = Queryable.AsQueryable(episodes).Average(e => e.TableAlterations);

                if (avg == 0)
                {
                    ItsTerminal?.Invoke(episodes.Min());
                    isworking = false;
                    return;
                }
                else
                {
                    episodes.RemoveLast();
                    episodes.AddFirst(currentEpisode);
                }
            }
            else
                episodes.AddFirst(currentEpisode);

            currentEpisode = new QLearningEpisode();
            ResetWorker();
        }

        public void ResetWorker()
        {
            randomCount = 0;
            worker.Steps = 0;
            worker.CurrentState = worker.IntialState;
        }
        

        public void Dispose()
        {
            States.Clear();
            worker = null;
        }
    }
}
