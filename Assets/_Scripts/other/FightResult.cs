using Assets.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets._Scripts.Enum
{
    public class FightResult
    {
        public bool Success { get; set; }
        public Dictionary<PowerType, int> ResultScore;
        public int finalSum;

        public FightResult()
        {
            ResultScore = new Dictionary<PowerType, int>();
            ResultScore.Add(PowerType.AGGRESSION, 0);
            ResultScore.Add(PowerType.CHARM, 0);
            ResultScore.Add(PowerType.LOGIC, 0);
            ResultScore.Add(PowerType.AUTHORITY, 0);
        }

        public void UpdateScore(PowerType powerType, int value)
        {
            ResultScore[powerType] = value;
        }

        public void Organize()
        {
            ResultScore = ResultScore.OrderBy(f => f.Value).ToDictionary(f => f.Key, f => f.Value);
        }

        public void SetFinalSum()
        {
            foreach(var f in ResultScore)
            {
                finalSum += f.Value;
            }

            if(finalSum > 0) Success = true; //Testowe gówno, później wyjebać XD
        }
    }
}
