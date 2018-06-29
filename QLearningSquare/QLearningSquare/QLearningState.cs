using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLearningSquare
{
    public enum StateType
    {
        AvoidState,
        GoalState,
        NormalState,
        Invalid
    }

    public class QLearningState : INotifyPropertyChanged
    {
        private Dictionary<string, QLearningAction> actions;

        private int stateReward;
        private string name;
        private StateType type;

        public int StateReward { get => stateReward;
            set
            {
                if (stateReward != value)
                {
                    stateReward = value;
                    RaisePropertyChanged("StateReward");
                }
            }
        }
        public string Name { get => name;
            set
            {
                if (name != value)
                {
                    name = value;
                    RaisePropertyChanged("Name");
                }
            }
        }
        public Dictionary<string, QLearningAction> Actions { get => actions;
            set
            {
                actions = value;
                RaisePropertyChanged("Actions");
            }
        }

        public StateType Type { get => type;
            set
            {
                if (type != value)
                {
                    type = value;
                    RaisePropertyChanged("Type");
                }
            }
        }

        public void AddAction(QLearningAction action)
        {
            Actions.Add(action.Name, action);
            RaisePropertyChanged("Actions["+ action.Name+"]");
        }

        public QLearningAction GetAction(string actionName)
        {
            if (Actions.ContainsKey(actionName))
                return Actions[actionName];
            else
                return null;
        }

        public int GetActionsCount()
        {
            return Actions.Count;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
    }
}
