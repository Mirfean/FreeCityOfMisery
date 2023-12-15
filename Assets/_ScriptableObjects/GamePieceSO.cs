using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GamePieceData", menuName = "Duck/GamePieceData"), Serializable]
public class GamePieceSO : ScriptableObject
{
    public int ID;

    public string cardName;

    public int effectId;

    [Range(0, 4)] public int cardLevel;

    public List<PowerType> VerticalSide;
    public List<PowerType> LeftSide;
    public List<PowerType> RightSide;

    public List<CardEffect> cardEffects;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
