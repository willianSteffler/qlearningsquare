using System;
using System.ComponentModel;

namespace QLearningSquare
{
    public class QLearningAction : INotifyPropertyChanged , IComparable<QLearningAction>
    {
        private string name;
        private string stateResult;
        private double reward;

        public double Reward { get => reward;
            set
            {
                if (reward != value)
                {
                    reward = value;
                    RaisePropertyChanged("Reward");
                }
            }
        }

        public string StateResult { get => stateResult;
            set
            {
                if (stateResult != value)
                {
                    stateResult = value;
                    RaisePropertyChanged("StateResult");
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

        public event PropertyChangedEventHandler PropertyChanged;

        public int CompareTo(QLearningAction other)
        {
            if (reward > other.reward)
                return 1;
            else if (reward < other.reward)
                return -1;

            return 0;
        }

        private void RaisePropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
    }
}