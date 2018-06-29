using QLearningSquare.GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLearningSquare.DAO;
using Shared;

namespace QLearningSquare.AppMediator
{
    class Mediator
    {
        private static Mediator mediator = new Mediator();

        //initialize classes
        public IGUIController pGUI = new GUIControl();
        public DataAcessObject pDAO = new DataAcessObject();
        QLearningController QLCtrl = new QLearningController();

        public static Mediator pMediator { get => mediator; }

        bool parametersLoaded = false;

        public Mediator()
        {
            pGUI.OnGuiLoaded = delegate ()
            {
                try
                {
                    pDAO.loadParameters(@"Parameters.json");
                }
                catch(Exception e)
                {
                    LogHelper.cat("Mediator", "Exception loading parameters -> " + e.Message);
                    pGUI.OnError("Falha ao abrir arquivo de parametros");
                }

                Init();
            };
        }

        public void Init()
        {
            try
            {

                //load the state Rewards and names
                List<List<int>> rewards = pDAO.getStateRewards();
                List<List<QLearningState>> states = new List<List<QLearningState>>();
                
                bool haveLeft;
                bool haveRight;
                bool haveUp;
                bool haveDown;

                int n = 0;
                for (int i = 0; i < rewards.Count; i++)
                {

                    List<QLearningState> row = new List<QLearningState>();
                    states.Add(row);

                    for (int j = 0; j < rewards[i].Count; j++)
                    {
                        haveUp = !(i == 0) && (rewards[i - 1].Count > j);
                        haveDown = !(i == rewards.Count - 1) && (rewards[i + 1].Count > j);
                        haveLeft = !(j == 0);
                        haveRight = !(j == rewards[i].Count - 1);

                        QLearningState s = new QLearningState();
                        s.Name = "S" + (++n);
                        s.StateReward = rewards[i][j];
                        s.Actions = new Dictionary<string, QLearningAction>();
                        s.Type = GetStateType(rewards[i][j]);

                        if (haveUp)
                            s.Actions["up"] = new QLearningAction() { Name = "up", Reward = pDAO.getActionReward(s.Name, "up"), StateResult = "S" + (n - rewards[i - 1].Count) };
                        if (haveDown)
                            s.Actions["down"] = new QLearningAction() { Name = "down", Reward = pDAO.getActionReward(s.Name, "down"), StateResult = "S" + (n + rewards[i + 1].Count) };
                        if (haveLeft)
                            s.Actions["left"] = new QLearningAction() { Name = "left", Reward = pDAO.getActionReward(s.Name, "left"), StateResult = "S" + (n - 1) };
                        if (haveRight)
                            s.Actions["right"] = new QLearningAction() { Name = "right", Reward = pDAO.getActionReward(s.Name, "right"), StateResult = "S" + (n + 1) };

                        QLCtrl.AddState(s);
                        row.Add(s);
                    }
                }

                string sinitialState = pDAO.getInitialStateName();
                QLCtrl.setInitialState(sinitialState);
                pGUI.SetStatesMatrix(states);
                pGUI.setWorkerState(QLCtrl.GetState(sinitialState));

                parametersLoaded = true;
            }
            catch (Exception e)
            {
                LogHelper.cat("Mediator", "Exception loading parameters -> " + e.Message);
                pGUI.OnError("Falha na inicialização dos parâmetros dos estados");
            }
        }

        public StateType GetStateType(int stateReward)
        {
            if (stateReward == -1)
                return StateType.NormalState;
            else if (stateReward == -100)
                return StateType.AvoidState;
            else if (stateReward == 100)
                return StateType.GoalState;
            else
                return StateType.Invalid;
        }

        
    }
}
