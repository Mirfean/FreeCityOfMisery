using Assets.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.ScriptableObjects
{
    [CreateAssetMenu(fileName = "IconPowerData", menuName = "Duck/IconPowerData"), Serializable]
    public class Icon_Power : ScriptableObject
    {
        public PowerType power;
        public Sprite symbol;
    }
}
