using Assets._Scripts.Enum;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "CURRENTGAMEDATA", menuName = "Duck/GameData"), Serializable]
public class CurrentGameData : ScriptableObject
{
    public string CharacterName;
    public string CurrentEnemyName;

    public DeckSO PlayerDeck;
    public DeckSO EnemyDeck;
    public GridData _GridData;
    public int RoundLimiter = 2;

    public bool LastFightWin;
    public FightResult FightResult;

    void SetDecks(DeckSO playerDeck, DeckSO enemyDeck)
    {

    }


}
