using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DeckData", menuName = "Duck/DeckData"), Serializable]
public class DeckSO : ScriptableObject
{
    public string DeckName;
    public List<GamePieceSO> Cards;
}
