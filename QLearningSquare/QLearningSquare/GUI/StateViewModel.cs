using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLearningSquare.GUI
{
    public class StateViewModel : INotifyPropertyChanged
    {
        private string name;
        private string statePoints;
        private string pointsLeft;
        private string pointsRight;
        private string pointsUp;
        private string pointsDown;

        public string Name
        {
            get => name;
            set
            {
                if (name != value)
                {
                    name = value;
                    RaisePropertyChanged("Name");
                }
            }
        }
        public string StatePoints
        {
            get => statePoints;
            set
            {
                if (statePoints != value)
                {
                    statePoints = value;
                    RaisePropertyChanged("StatePoints");
                }
            }
        }

        public string PointsLeft
        {
            get => pointsLeft;
            set
            {
                if (pointsLeft != value)
                {
                    pointsLeft = value;
                    RaisePropertyChanged("PointsLeft");
                }
            }
        }
        public string PointsRight
        {
            get => pointsRight;
            set
            {
                if (pointsRight != value)
                {
                    pointsRight = value;
                    RaisePropertyChanged("PointsRight");
                }
            }
        }
        public string PointsDown
        {
            get => pointsDown;
            set
            {
                if (pointsDown != value)
                {
                    pointsDown = value;
                    RaisePropertyChanged("PointsDown");
                }
            }
        }
        public string PointsUp
        {
            get => pointsUp;
            set
            {
                if (pointsUp != value)
                {
                    pointsUp = value;
                    RaisePropertyChanged("PointsUp");
                }
            }
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
