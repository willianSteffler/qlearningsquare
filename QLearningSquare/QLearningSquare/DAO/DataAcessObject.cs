using JsonMaker;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLearningSquare.DAO
{
    class DataAcessObject
    {
        JSON parameters = new JSON();

        public void loadParameters(string url)
        {
            string jsonString = File.ReadAllText(url);
            parameters.fromJson(jsonString);
        }

        public List<List<int>> getStateRewards()
        {
            int rows = parameters.getChildsNames("StateRewards").Count;
            int columns;
            List<List<int>> rewards = new List<List<int>>();

            for (int i = 0; i < rows; i++)
            {
                List<int> row = new List<int>();
                rewards.Add(row);

                columns = parameters.getChildsNames("StateRewards." + i).Count;
                for (int j = 0; j < columns; j++)
                {
                    rewards[i].Add(parameters.getInt("StateRewards." + i + "." + j));
                }
            }

            return rewards;
        }

        public int getActionReward(string stateName, string action,int defValue = 0)
        {
            return defValue;
        }

        public string getInitialStateName()
        {
            return parameters.getString("InitialState");
        }
    }
}
