using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightManager : MonoBehaviour
{
    [SerializeField] List<DeckSO> decks;
    [SerializeField] CurrentGameData gameData;
    
    [SerializeField] DeckManager deckManager;
    [SerializeField] ScoreManager scoreManager;

    [SerializeField] EndFightScreen endScreenData;

    [SerializeField] GameHand hand;

    public static Action ENDFIGHT;

    private void Awake()
    {
        //if it's fight scene
        BattlePreparer();

        ENDFIGHT += EndFight;
    }

    private void OnDestroy()
    {
        ENDFIGHT -= EndFight;
    }

    // Start is called before the first frame update
    void Start()
    {
        deckManager.DrawToFullHandPlayer();
        deckManager.DrawToFullHandEnemy();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void BattlePreparer()
    {
        LoadDecks();
    }

    void LoadDecks()
    {
        deckManager.PreparePlayerDeck(gameData.PlayerDeck);
        deckManager.PrepareEnemyDeck(gameData.EnemyDeck);
    }

    public void EndFight()
    {
        //Compare score -> get a winner
        var finalScore = scoreManager.GetFinalScore();
        gameData.FightResult = finalScore;
        gameData.LastFightWin = finalScore.Success;

        endScreenData.SetPlayerData(gameData.CharacterName, scoreManager.PlayerScore.GetSum());
        endScreenData.SetEnemyData(gameData.CurrentEnemyName, scoreManager.EnemyScore.GetSum());

        if (finalScore.Success)
        {
            // Send info win/lose + order of most used powers(for future)
        }

        //Effects


        //Hide scene by using method from game manager
        GameManager.HideFight();
    }
}
