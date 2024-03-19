using Assets.Scripts;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "CardEffect", menuName = "Duck/CardEffect"), Serializable]
public class CardEffect : ScriptableObject
{
    public SpecialEffects effect;
    public PowerType powerType;
    public int value;
    public string description;
}
