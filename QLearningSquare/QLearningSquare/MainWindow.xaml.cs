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
        public QLearningState ActualStateInfo { get; set; }
        public ObservableCollection<QLearningState> Qtable { get; set; }

        private int animateInterval;
        private bool autoAnimate;
        private double gridStatesSize;
        private int currentsteps;


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

        public double GridStatesSize { get => gridStatesSize;
            set
            {
                gridStatesSize = value;
                
                RaisePropertyChanged("GridStatesSize");
            }
        }

        public int CurrentSteps
        {
            get => currentsteps;
            set
            {
                currentsteps = value;

                RaisePropertyChanged("CurrentSteps");
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
        public MainWindowViewModel ViewModel { get; set; }
        GUIControl ctrl = (GUIControl)AppMediator.Mediator.pMediator.pGUI;

        public MainWindow()
        {
            InitializeComponent();
            ViewModel = new MainWindowViewModel();
            this.DataContext = ViewModel;
            ctrl.pMainWindow = this;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ctrl.onMainWindowLoaded();
        }

        private void gridStates_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double maxValue = e.NewSize.Height < (e.NewSize.Width - 300) ? e.NewSize.Height : (e.NewSize.Width - 300);

            ViewModel.GridStatesSize = gridStates.ColumnDefinitions.Count > gridStates.RowDefinitions.Count ?
                maxValue / gridStates.ColumnDefinitions.Count : maxValue / gridStates.RowDefinitions.Count;
            
        }
    }
}
