using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardEffect", menuName = "Duck/CardEffect"), Serializable]
public class CardEffect : ScriptableObject
{
    public SpecialEffects effect;
    public PowerType powerType;
    public int value;
}
