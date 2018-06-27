using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using QLearningSquare.GUI;

namespace QLearningSquare
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public StateViewModel ActualStateInfo = new StateViewModel();
        public ObservableCollection<StateViewModel> Qtable = new ObservableCollection<StateViewModel>();

        private int animateInterval;
        private bool autoAnimate;

        
        public int AnimateInterval { get => animateInterval;
            set
            {
                if (animateInterval != value)
                {
                    animateInterval = value;
                    RaisePropertyChanged("AnimateInterval");
                }
            }
        }
        public bool AutoAnimate { get => autoAnimate;
            set
            {
                if (autoAnimate != value)
                {
                    autoAnimate = value;
                    RaisePropertyChanged("AutoAnimate");
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

    /// <summary>
    /// Interação lógica para MainWindow.xam
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindowViewModel ViewModel = new MainWindowViewModel();
        GUIControl ctrl = (GUIControl)AppMediator.Mediator.pMediator.pGUI;

        public MainWindow()
        {
            InitializeComponent();
            ctrl.pMainWindow = this;
            this.DataContext = ViewModel;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < length; i++)
            {

            }
            ctrl.onMainWindowLoaded();
        }
    }
}
