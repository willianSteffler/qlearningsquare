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

        public bool loadParameters(string url)
        {
            try
            {
                string jsonString = File.ReadAllText(url);
                parameters.fromJson(jsonString);
                return true;
            }
            catch { }

            return false;
        }

        public List<List<int>> getStateRewards()
        {
            List<List<int>> rewards = new List<List<int>>();
            for (int i = 0; i < parameters.getChildsNames("StateRewards").Count; i++)
            {
                rewards[i] = new List<int>();
                for(int j = 0; j < parameters.getChildsNames("StateRewards."+i).Count; j++)
                {
                    rewards[i][j] = parameters.getInt("StateRewards." + i + "." + j);
                }
            }

            return rewards;
        }

    }
}
