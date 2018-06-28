using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace QLearningSquare.GUI
{

    class GUIControl : IGUIController
    {
        class GridPosition
        {
            public int Row;
            public int Column;
        }

        Dictionary<string, GridPosition> statePositions = new Dictionary<string,GridPosition>();

        AppMediator.Mediator ctrl = AppMediator.Mediator.pMediator;
        public MainWindow pMainWindow;

        Action onGUILoaded = null;
        public Action OnGuiLoaded { get => onGUILoaded; set => onGUILoaded = value; }

        public void MoveSquare(string state)
        {
            throw new NotImplementedException();
        }

        public void OnError(string errorMessage)
        {
            MessageBox.Show(errorMessage);
        }

        public void onMainWindowLoaded()
        {
            onGUILoaded();
        }

        public void SetStatesMatrix(List<List<QLearningState>> statesMatrix)
        {
            pMainWindow.ViewModel.Qtable = new ObservableCollection<QLearningState>();

            for (int i = 0; i < statesMatrix.Count; i++)
            {
                if (pMainWindow.gridStates.RowDefinitions.Count < statesMatrix.Count)
                {
                    pMainWindow.gridStates.RowDefinitions.Add(new RowDefinition());
                }
                for (int j = 0; j < statesMatrix[i].Count; j++)
                {
                    if(pMainWindow.gridStates.ColumnDefinitions.Count < statesMatrix[i].Count)
                    {
                        pMainWindow.gridStates.ColumnDefinitions.Add(new ColumnDefinition());
                    }

                    pMainWindow.ViewModel.Qtable.Add(statesMatrix[i][j]);
                    statePositions[statesMatrix[i][j].Name] = new GridPosition() { Row = i, Column = j };

                    Border b = new Border();
                    
                    b.Style = (Style)pMainWindow.FindResource(statesMatrix[i][j].Type.ToString());
                    pMainWindow.gridStates.Children.Add(b);
                    Grid.SetRow(b, i);
                    Grid.SetColumn(b, j);
                }
            }

            pMainWindow.QtableView.ItemsSource = pMainWindow.ViewModel.Qtable;
            pMainWindow.ViewModel.GridStatesSize = 70;

        }
    }
}
