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
                        haveUp = !(i == 0);
                        haveDown = !(i == rewards.Count -1);

                        List<QLearningState> row = new List<QLearningState>();
                        states.Add(row);

                        for (int j = 0; j < rewards[i].Count; j++)
                        {
                            haveLeft = !(j == 0);
                            haveRight = !(j == rewards[i].Count - 1);

                            QLearningState s = new QLearningState();
                            s.Name = "S" + (++n);
                            s.StateReward = rewards[i][j];
                            s.Actions = new Dictionary<string, QLearningAction>();

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
                        n++;
                    }

                    pGUI.SetStatesMatrix(states);
                    parametersLoaded = true;
                }
                catch(Exception e)
                {
                    LogHelper.cat("Mediator", "Exception loading parameters -> " + e.Message);
                    pGUI.OnError("Falha na inicialização dos parâmetros dos estados");
                }

            };
        }
    }
}
