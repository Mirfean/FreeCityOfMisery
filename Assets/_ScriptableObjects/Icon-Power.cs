using Assets.Scripts;
using System;
using UnityEngine;

namespace Assets.ScriptableObjects
{
    [CreateAssetMenu(fileName = "IconPowerData", menuName = "Duck/IconPowerData"), Serializable]
    public class Icon_Power : ScriptableObject
    {
        public PowerType power;
        public Sprite symbol;
    }
}
