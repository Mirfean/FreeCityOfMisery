using Assets._Scripts.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "CURRENTGAMEDATA", menuName = "Duck/GameData"), Serializable]
public class CurrentGameData: ScriptableObject
{
    public string CharacterName;
    public string CurrentEnemyName;

    public DeckSO PlayerDeck;
    public DeckSO EnemyDeck;
    public GridData _GridData;

    public bool LastFightWin;
    public FightResult FightResult;

    void SetDecks(DeckSO playerDeck, DeckSO enemyDeck)
    {

    }


}
